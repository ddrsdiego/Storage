namespace Rydo.Storage.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Read;
    using StackExchange.Redis;
    using Storage.Extensions;

    internal sealed class RedisReadStorageContentProvider : DbReadStorageContentProvider
    {
        private IRedisStorageService? _redisServiceCache;

        public RedisReadStorageContentProvider(IModelTypeContextContainer modelTypeContextContainer)
            : base(modelTypeContextContainer)
        {
        }

        public override async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            if (!ModelTypeContextContainer.TryGetModel(batch.ModeTypeName, out var modelTypeContext))
            {
                // What Happened: Unable to locate settings for Model: Test
                // Why Happened An attempt was made to read the Test model but it was not configured properly
                // Review the Model Test settings, it may be that the model has not been included in the settings.
            }

            _redisServiceCache = (IRedisStorageService) modelTypeContext.StorageService;

            await Observable.PreExecuteRead(batch);

            var readTasks = InitReadTasks(batch, batch.TableName);
            foreach (var (request, task) in readTasks)
            {
                try
                {
                    var redisValue = await task.WaitCompletedSuccessfully();

                    var response = CreateReadResponse(redisValue, request);

                    _ = request.Response.TrySetResult(response);
                }
                catch (Exception e)
                {
                    _ = request.Response.TrySetResult(ReadResponse.GetResponseError(request, e));
                }
            }

            await Observable.PostExecuteRead(batch);

            await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        }

        private static ReadResponse CreateReadResponse(RedisValue redisValue, ReadRequest readRequest)
        {
            if (!redisValue.HasValue || redisValue.IsNullOrEmpty)
                return ReadResponse.GetResponseNotFound(readRequest);

            return ReadResponse.GetResponseOk(readRequest, (byte[]) redisValue.Box()!);
        }

        private IDictionary<ReadRequest, Task<RedisValue>> InitReadTasks(IReadBatchRequest batch, string modelTypeName)
        {
            var readTasks = new Dictionary<ReadRequest, Task<RedisValue>>(batch.Count);
            var requests = batch.ToArray();

            for (var index = 0; index < requests.Length; index++)
            {
                var storageItemKey = requests[index].ToStorageItemKey();
                var query = (RedisValue) storageItemKey.Value;

                var task = _redisServiceCache?.Reader.HashGetAsync(modelTypeName, query);
                if (task != null) readTasks.Add(requests[index], task);
            }

            return readTasks;
        }
    }
}