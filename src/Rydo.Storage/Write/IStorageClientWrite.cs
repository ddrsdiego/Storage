namespace Rydo.Storage.Write
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IStorageClientWrite
    {
        /// <summary>
        /// Write to the database returning a response containing the result of the operation.
        /// </summary>
        /// <param name="key">Unique identification key for the resource that will be created in the database</param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Upsert<T>(string key, T payload, CancellationToken cancellationToken = default);

        /// <summary>
        /// Write to the database returning a response containing the result of the operation.
        /// </summary>
        /// <param name="key">Unique identification key for the resource that will be created in the database</param>
        /// <param name="sortKey"></param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Upsert<T>(string key, string sortKey, T payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Unique identification key for the resource that will be created in the database</param>
        /// <param name="payload"></param>
        /// <param name="futureWriteResponse"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask UpsertAsync<T>(string key, T payload, FutureWriteResponse futureWriteResponse);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Unique identification key for the resource that will be created in the database</param>
        /// <param name="sortKey"></param>
        /// <param name="payload"></param>
        /// <param name="futureWriteResponse"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask UpsertAsync<T>(string key, string sortKey, T payload, FutureWriteResponse futureWriteResponse);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Unique identification key for the resource that will be created in the database</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Remove<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Unique identification key for the resource that will be created in the database</param>
        /// <param name="sortKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Remove<T>(string key, string sortKey);
    }
}