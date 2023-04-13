namespace Rydo.Storage.Redis
{
    using Microsoft.Extensions.Logging;
    using Providers;
    using Read;
    using Write;

    internal sealed class RedisStorageContentProvider : StorageContentProvider
    {
        public RedisStorageContentProvider(ILoggerFactory logger,
            IDbWriteStorageContentProvider writeStorageContentProvider,
            IDbReadStorageContentProvider readStorageContentProvider)
            : base(logger.CreateLogger<RedisStorageContentProvider>(), writeStorageContentProvider,
                readStorageContentProvider)
        {
        }
    }
}