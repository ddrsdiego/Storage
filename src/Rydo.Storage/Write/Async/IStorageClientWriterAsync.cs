namespace Rydo.Storage.Write.Async
{
    using System.Threading.Tasks;

    public interface IStorageClientWriterAsync
    {
        ValueTask UpsertAsync<T>(string key, T payload, FutureWriteResponse futureWriteResponse);

        ValueTask UpsertAsync<T>(string key, string sortKey, T payload, FutureWriteResponse futureWriteResponse);
    }
}