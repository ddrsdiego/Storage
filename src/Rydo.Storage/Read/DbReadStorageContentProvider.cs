namespace Rydo.Storage.Read
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Observers;
    using Abstractions.Observers.Observables;
    using Abstractions.Utils;
    using Extensions;

    public abstract class DbReadStorageContentProvider :
        IDbReadStorageContentProvider
    {
        protected readonly IModelTypeContextContainer ModelTypeContextContainer;
        protected readonly DbReadStorageContentProviderObservable Observable;

        protected DbReadStorageContentProvider(IModelTypeContextContainer modelTypeContextContainer)
        {
            ModelTypeContextContainer = modelTypeContextContainer;
            Observable = new DbReadStorageContentProviderObservable();
        }

        public IConnectHandle ConnectDbReadStorageContentProviderObserver(
            IDbReadStorageContentProviderObserver observer)
        {
            return Observable.Connect(observer);
        }

        public abstract Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);
    }
}