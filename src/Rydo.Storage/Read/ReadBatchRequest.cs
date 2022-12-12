namespace Rydo.Storage.Read
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public interface IReadBatchRequest : IBatchRequest, IEnumerable<ReadRequest>
    {
        bool TryAdd(ReadRequest readRequest);
    }

    public sealed class ReadBatchRequest : IReadBatchRequest
    {
        private long _nextId;
        private readonly object _syncLoc;
        private readonly Dictionary<long, ReadRequest> _readRequests;

        internal ReadBatchRequest(int capacity)
        {
            _syncLoc = new object();
            _readRequests = new Dictionary<long, ReadRequest>(capacity);

            BatchId = Guid.NewGuid().ToString().Split('-')[0];
        }

        private string _modeTypeName = string.Empty;

        public string ModeTypeName
        {
            get
            {
                if (string.IsNullOrEmpty(_modeTypeName))
                    _modeTypeName = _readRequests.Values.First().Definition.ModeTypeName;
                return _modeTypeName;
            }
        }

        public bool TryAdd(ReadRequest readRequest) => InternalTryAdd(readRequest);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool InternalTryAdd(ReadRequest readRequest)
        {
            var isNewToken = true;

            var id = Interlocked.Increment(ref _nextId);

            lock (_syncLoc)
            {
                var key = readRequest.ToStorageItemKey().Value!;

                if (_readRequests.TryGetValue(id, out _))
                    isNewToken = false;

                _readRequests.Add(id, readRequest);
            }

            Interlocked.Increment(ref _counter);

            return isNewToken;
        }

        public string BatchId { get; }

        private int _counter;
        public int Count => _counter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerator<ReadRequest> GetEnumerator()
        {
            foreach (var readRequest in _readRequests)
            {
                yield return readRequest.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}