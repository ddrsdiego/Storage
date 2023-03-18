namespace Rydo.Storage.Memory
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Write;

    public interface IStorageMemory
    {
        public IStorageMemoryRead Reader { get; }
        public IStorageMemoryWrite Writer { get; }

        ValueTask Upsert(IWriteBatchRequest writeBatchRequest);
    }

    public sealed class StorageMemory : IStorageMemory
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StorageMemory> _logger;

        public StorageMemory(ILogger<StorageMemory> logger,
            IMemoryCache memoryCache,
            IStorageMemoryWrite storageMemoryWrite,
            IStorageMemoryRead storageMemoryRead)
        {
            Writer = storageMemoryWrite;
            Reader = storageMemoryRead;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public IStorageMemoryRead Reader { get; }
        public IStorageMemoryWrite Writer { get; }

        public ValueTask Upsert(IWriteBatchRequest writeBatchRequest) => Writer.Upsert(writeBatchRequest);
    }
}