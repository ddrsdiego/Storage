namespace Rydo.Storage.Write.Sync
{
    using System.Threading.Tasks;

    public interface IStorageClientWriteSync
    {
        ValueTask<WriteResponse> Upsert<T>(string key, T payload);

        ValueTask<WriteResponse> Upsert<T>(string key, string sortKey, T payload);

        ValueTask<WriteResponse> Remove<T>(string key);

        ValueTask<WriteResponse> Remove<T>(string key, string sortKey);
    }
}