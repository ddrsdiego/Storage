namespace Rydo.Storage.Redis
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Read;
    using Write;

    internal sealed class RedisStorageContentProvider : StorageContentProvider
    {
        private readonly IRedisReadStorageContentProvider _readStorageContentProvider;
        private readonly IRedisWriteStorageContentProvider _writeStorageContentProvider;

        public RedisStorageContentProvider(ILoggerFactory logger,
            IRedisWriteStorageContentProvider writeStorageContentProvider,
            IRedisReadStorageContentProvider readStorageContentProvider)
            : base(logger.CreateLogger<RedisStorageContentProvider>())
        {
            _writeStorageContentProvider = writeStorageContentProvider;
            _readStorageContentProvider = readStorageContentProvider;
        }

        public override Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            return _readStorageContentProvider.Read(batch, cancellationToken);
        }

        public override Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            return _writeStorageContentProvider.Upsert(writeBatchRequest);
        }
    }
}