namespace Rydo.Storage.Memory
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Write;

    public interface IStorageMemoryWrite
    {
        ValueTask Upsert(IWriteBatchRequest writeBatchRequest);
    }

    internal sealed class StorageMemoryWrite : IStorageMemoryWrite
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StorageMemoryWrite> _logger;

        public StorageMemoryWrite(IMemoryCache memoryCache, ILogger<StorageMemoryWrite> logger)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public ValueTask Upsert(IWriteBatchRequest writeBatchRequest)
        {
            try
            {
                var requests = writeBatchRequest.ToArray();
                
                for (var index = 0; index < requests.Length; index++)
                {
                    var writeRequest = requests[index];
                    
                    var storageItem = writeRequest.ToStorageItem();
                    _memoryCache.Set(storageItem.Key.Value, storageItem.Payload, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = writeRequest.ModelTypeDefinition.TimeToLive
                    });
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