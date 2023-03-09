namespace Rydo.Storage.DynamoDB
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Read;
    using Storage.Read;
    using Storage.Write;
    using Write;

    public sealed class DynamoDbStorageContentProvider : StorageContentProvider
    {
        private readonly IDbReadStorageContentProvider _dbReadStorageContentProvider;
        private readonly IDbWriteStorageContentProvider _dbWriteStorageContentProvider;

        public DynamoDbStorageContentProvider(ILoggerFactory logger,
            IDbWriteStorageContentProvider dbWriteStorageContentProvider,
            IDbReadStorageContentProvider dbReadStorageContentProvider)
            : base(logger.CreateLogger<DynamoDbStorageContentProvider>())
        {
            _dbReadStorageContentProvider = dbReadStorageContentProvider;
            _dbWriteStorageContentProvider = dbWriteStorageContentProvider;
        }

        public override Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            return _dbReadStorageContentProvider.Read(batch, cancellationToken);
        }

        public override Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            return _dbWriteStorageContentProvider.Write(writeBatchRequest, cancellationToken);
        }
    }
}