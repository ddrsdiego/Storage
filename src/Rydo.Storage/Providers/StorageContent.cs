namespace Rydo.Storage.Providers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Read;
    using Write;

    public interface IStorageContentProvider
    {
        Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);

        Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default);
    }

    public abstract class StorageContentProvider : IStorageContentProvider
    {
        protected StorageContentProvider(ILogger logger, IDbWriteStorageContentProvider writeStorageContentProvider,
            IDbReadStorageContentProvider readStorageContentProvider)
        {
            Logger = logger;
            WriteStorageContentProvider = writeStorageContentProvider;
            ReadStorageContentProvider = readStorageContentProvider;
        }

        private ILogger Logger { get; }

        private IDbWriteStorageContentProvider WriteStorageContentProvider { get; }

        private IDbReadStorageContentProvider ReadStorageContentProvider { get; }

        public Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            return ReadStorageContentProvider.Read(batch, cancellationToken);
        }

        public Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            return WriteStorageContentProvider.Write(writeBatchRequest, cancellationToken);
        }
    }
}