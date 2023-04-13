namespace Rydo.Storage.DynamoDB
{
    using Microsoft.Extensions.Logging;
    using Providers;
    using Storage.Read;
    using Storage.Write;

    internal sealed class DynamoDbStorageContentProvider : StorageContentProvider
    {
        public DynamoDbStorageContentProvider(ILoggerFactory logger,
            IDbWriteStorageContentProvider writeStorageContentProvider,
            IDbReadStorageContentProvider readStorageContentProvider)
            : base(logger.CreateLogger<DynamoDbStorageContentProvider>(), writeStorageContentProvider,
                readStorageContentProvider)
        {
        }
    }
}