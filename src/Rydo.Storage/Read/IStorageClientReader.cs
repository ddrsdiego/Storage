namespace Rydo.Storage.Read
{
    using System.Threading.Tasks;

    public interface IStorageClientReader
    {
        ValueTask<ReadResponse> Read<T>(string key);

        ValueTask<ReadResponse> Read<T>(string key, string sortKey);

        ValueTask ReadAsync<T>(string key, FutureReadResponse futureReadResponse);

        ValueTask ReadAsync<T>(string key, string sortKey, FutureReadResponse futureReadResponse);
    }
}