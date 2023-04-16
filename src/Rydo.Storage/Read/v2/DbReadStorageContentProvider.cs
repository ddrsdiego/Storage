namespace Rydo.Storage.Read.v2
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions.Observers;
    using Abstractions.Observers.Observables;
    using Abstractions.Utils;
    using Providers;

    public abstract class DbReadStorageContentProvider :
        IDbReadStorageContentProvider
    {
        protected readonly ModelTypeContext ModelTypeContext;
        protected readonly DbReadStorageContentProviderObservable Observable;

        protected DbReadStorageContentProvider(ModelTypeContext modelTypeContext)
        {
            ModelTypeContext = modelTypeContext;
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