namespace Rydo.Storage.Write
{
    using System;
    using System.Threading.Tasks;
    using Async;
    using Sync;

    public sealed class StorageClientWrite : IStorageClientWrite
    {
        private readonly IStorageClientWriteSync _storageClientWriteSync;
        private readonly IStorageClientWriterAsync _storageClientWriterAsync;

        public StorageClientWrite(IStorageClientWriteSync storageClientWriteSync,
            IStorageClientWriterAsync storageClientWriterAsync)
        {
            _storageClientWriteSync = storageClientWriteSync ??
                                      throw new ArgumentNullException(nameof(storageClientWriteSync));

            _storageClientWriterAsync = storageClientWriterAsync ??
                                        throw new ArgumentNullException(nameof(storageClientWriterAsync));
        }

        public ValueTask<WriteResponse> Upsert<T>(string key, T payload)
        {
            return _storageClientWriteSync.Upsert(key, payload);
        }

        public ValueTask<WriteResponse> Upsert<T>(string key, string sortKey, T payload)
        {
            return _storageClientWriteSync.Upsert(key, sortKey, payload);
        }

        public ValueTask UpsertAsync<T>(string key, T payload, FutureWriteResponse futureWriteResponse)
        {
            return _storageClientWriterAsync.UpsertAsync(key, payload, futureWriteResponse);
        }

        public ValueTask UpsertAsync<T>(string key, string sortKey, T payload, FutureWriteResponse futureWriteResponse)
        {
            return _storageClientWriterAsync.UpsertAsync(key, sortKey, payload, futureWriteResponse);
        }

        public ValueTask<WriteResponse> Remove<T>(string key) => _storageClientWriteSync.Remove<T>(key);

        public ValueTask<WriteResponse> Remove<T>(string key, string sortKey) =>
            _storageClientWriteSync.Remove<T>(key, sortKey);
    }
}