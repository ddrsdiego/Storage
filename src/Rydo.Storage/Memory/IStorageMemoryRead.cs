namespace Rydo.Storage.Memory
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Read;

    public interface IStorageMemoryRead
    {
        ValueTask Read(ReadBatchRequest readBatchRequest);
    }
    
    internal sealed class StorageMemoryRead : IStorageMemoryRead
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StorageMemoryRead> _logger;

        public StorageMemoryRead(IMemoryCache memoryCache, ILogger<StorageMemoryRead> logger)
        {
            _logger = logger;
            _memoryCache = memoryCache;
        }
        
        public ValueTask Read(ReadBatchRequest readBatchRequest)
        {
            throw new System.NotImplementedException();
        }
    }
}