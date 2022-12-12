namespace Rydo.Storage.Memory
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Write;

    public interface IStorageMemory
    {
        ValueTask Upsert(IWriteBatchRequest writeBatchRequest);
    }

    public sealed class StorageMemory : IStorageMemory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StorageMemory> _logger;

        public StorageMemory(ILogger<StorageMemory> logger, IMemoryCache memoryCache)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public ValueTask Upsert(IWriteBatchRequest writeBatchRequest)
        {
            try
            {
                foreach (var writeRequest in writeBatchRequest)
                {
                    var storageItem = writeRequest.ToStorageItem();

                    _memoryCache.Set(storageItem.Key.Value, storageItem.Payload, writeRequest.ModelTypeDefinition!.TimeToLive);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }

            return new ValueTask(Task.CompletedTask);
        }
    }
}