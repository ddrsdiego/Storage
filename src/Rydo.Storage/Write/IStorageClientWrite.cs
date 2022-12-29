namespace Rydo.Storage.Write
{
    using System.Threading.Tasks;

    public interface IStorageClientWrite
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Upsert<T>(string key, T payload);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sortKey"></param>
        /// <param name="payload"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Upsert<T>(string key, string sortKey, T payload);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="payload"></param>
        /// <param name="futureWriteResponse"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask UpsertAsync<T>(string key, T payload, FutureWriteResponse futureWriteResponse);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sortKey"></param>
        /// <param name="payload"></param>
        /// <param name="futureWriteResponse"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask UpsertAsync<T>(string key, string sortKey, T payload, FutureWriteResponse futureWriteResponse);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Remove<T>(string key);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="sortKey"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ValueTask<WriteResponse> Remove<T>(string key, string sortKey);
    }
}