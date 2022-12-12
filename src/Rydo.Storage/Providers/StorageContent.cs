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
        protected StorageContentProvider(ILogger logger)
        {
            Logger = logger;
        }

        protected ILogger Logger { get; }

        public abstract Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);

        public abstract Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default);
    }
}