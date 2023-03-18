namespace Rydo.Storage.Read
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IStorageClientReader
    {
        ValueTask<ReadResponse> Read<T>(string key, CancellationToken cancellationToken = default);

        ValueTask<ReadResponse> Read<T>(string key, string sortKey, CancellationToken cancellationToken = default);

        ValueTask ReadAsync<T>(string key, FutureReadResponse futureReadResponse,
            CancellationToken cancellationToken = default);

        ValueTask ReadAsync<T>(string key, string sortKey, FutureReadResponse futureReadResponse,
            CancellationToken cancellationToken = default);
    }
}