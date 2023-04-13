namespace Rydo.Storage.Abstractions.Observers
{
    using System.Threading.Tasks;
    using Read;

    public interface IDbReadStorageContentProviderObserver
    {
        Task PreExecuteRead(ReadBatchRequest batch);
        
        Task PostExecuteRead(ReadBatchRequest batch);
    }
}