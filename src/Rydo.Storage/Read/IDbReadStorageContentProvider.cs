namespace Rydo.Storage.Read
{
    using System.Threading;
    using System.Threading.Tasks;
    using Observers;

    public interface IDbReadStorageContentProvider :
        IDbReadStorageContentProviderObserverConnector
    {
        Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);
    }
}