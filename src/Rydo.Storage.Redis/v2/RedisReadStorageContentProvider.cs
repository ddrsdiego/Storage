namespace Rydo.Storage.Redis.v2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Read;
    using StackExchange.Redis;
    using Storage.Extensions;
    using Write;

    internal sealed class  RedisStorageContentProvider : StorageContentProvider
    {
        public RedisStorageContentProvider(ILoggerFactory logger,
            IDbWriteStorageContentProvider writeStorageContentProvider,
            IDbReadStorageContentProvider readStorageContentProvider)
            : base(logger.CreateLogger<RedisStorageContentProvider>(), writeStorageContentProvider,
                readStorageContentProvider)
        {
        }
    }
    
    internal sealed class RedisReadStorageContentProvider : Rydo.Storage.Read.v2.DbReadStorageContentProvider
    {
        private IRedisStorageService? _redisServiceCache;

        public RedisReadStorageContentProvider(ModelTypeContext modelTypeContext)
            : base(modelTypeContext)
        {
        }

        public override async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            _redisServiceCache = (IRedisStorageService) ModelTypeContext.StorageService;

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