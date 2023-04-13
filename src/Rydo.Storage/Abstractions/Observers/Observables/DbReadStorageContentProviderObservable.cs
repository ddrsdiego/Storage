namespace Rydo.Storage.Abstractions.Observers.Observables
{
    using System.Threading.Tasks;
    using Read;
    using Utils;

    public class DbReadStorageContentProviderObservable :
        Connectable<IDbReadStorageContentProviderObserver>,
        IDbReadStorageContentProviderObserver
    {
        public Task PreExecuteRead(ReadBatchRequest batch)
        {
            return Count <= 0 ?
                Task.CompletedTask :
                ForEachAsync(x => x.PreExecuteRead(batch));
        }

        public Task PostExecuteRead(ReadBatchRequest batch)
        {
            return Count <= 0 ?
                Task.CompletedTask :
                ForEachAsync(x => x.PostExecuteRead(batch));
        }
    }
}