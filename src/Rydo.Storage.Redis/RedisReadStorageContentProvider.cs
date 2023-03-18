namespace Rydo.Storage.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Read;
    using StackExchange.Redis;
    using Storage.Extensions;

    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    internal sealed class RedisReadStorageContentProvider : IDbReadStorageContentProvider
    {
        private readonly ILogger<RedisReadStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;
        private IRedisStorageService? _redisServiceCache;

        public RedisReadStorageContentProvider(ILogger<RedisReadStorageContentProvider> logger,
            IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger;
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            if (!_modelTypeContextContainer.TryGetModel(batch.ModeTypeName!, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _redisServiceCache = (IRedisStorageService) modelTypeContext.StorageService;

            var sw = Stopwatch.StartNew();
            _logger.LogInformation("[{BatchId}] - Starting query for {BatchCount} item(s) on {TableName} table", 
                batch.BatchId,
                batch.Count,
                batch.TableName);

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

            _logger.LogInformation("[{BatchId}] - Finishing query for {BatchCount} item(s) on {TableName} table, Elapsed Time: {ElapsedMilliseconds}", 
                batch.BatchId,
                batch.Count,
                batch.TableName,
                sw.ElapsedMilliseconds);
            
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