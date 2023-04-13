namespace Rydo.Storage.Write
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using Helpers;

    public interface IWriteBatchRequest : IBatchRequest, IEnumerable<WriteRequest>
    {
        string ModeTypeName { get; }

        string TableName { get; }

        IEnumerable<StorageItem> StorageItems { get; }

        void TryAdd(WriteRequest writeRequest);
    }

    internal sealed class WriteBatchRequest : IWriteBatchRequest
    {
        private readonly object _sync = new object();
        private readonly Dictionary<string, WriteRequest> _writeRequests;

        internal WriteBatchRequest(int capacity)
        {
            BatchId = BatchId = $"B-W-ID-{GeneratorOperationId.Generate()}";
            ;
            _writeRequests = new Dictionary<string, WriteRequest>(capacity);
        }

        private string _modeTypeName = string.Empty;

        public string ModeTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(_modeTypeName))
                {
                    _modeTypeName = _writeRequests.First().Value.ModelTypeDefinition?.ModeTypeName;
                }

                return _modeTypeName;
            }
        }

        private string _tableName = string.Empty;

        public string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_tableName))
                {
                    _tableName = _writeRequests.First().Value.ModelTypeDefinition?.TableName!;
                }

                return _tableName;
            }
        }

        public string BatchId { get; }

        private int _count;

        public int Count
        {
            get
            {
                lock (_sync)
                {
                    return _count;
                }
            }
        }

        public void TryAdd(WriteRequest writeRequest) => InternalAdd(writeRequest);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool InternalAdd(WriteRequest writeRequest)
        {
            lock (_sync)
            {
                if (writeRequest.IsInvalidRequest) return false;

                var key = writeRequest.ToStorageItem().Key.Value;

                if (_writeRequests.TryGetValue(key, out _)) return false;

                _writeRequests[key] = writeRequest;
                Interlocked.Increment(ref _count);
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<WriteRequest> GetEnumerator()
        {
            lock (_sync)
            {
                foreach (var writeRequest in _writeRequests)
                {
                    if (writeRequest.Value.IsValidRequest)
                        yield return writeRequest.Value;
                }
            }
        }

        public IEnumerable<StorageItem> StorageItems
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                foreach (var writeRequest in this)
                {
                    yield return writeRequest.ToStorageItem();
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}