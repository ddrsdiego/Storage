namespace Rydo.Storage.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Write;
    using StackExchange.Redis;
    using Storage.Extensions;

    internal sealed class RedisWriteStorageContentProvider : IDbWriteStorageContentProvider
    {
        private const int DelayInMilliseconds = 1;
        
        private IRedisStorageService? _redisServiceCache;
        private readonly ILogger<RedisWriteStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public RedisWriteStorageContentProvider(ILogger<RedisWriteStorageContentProvider> logger,
            IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger;
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public async Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            if (!_modelTypeContextContainer.TryGetModel(writeBatchRequest.ModeTypeName, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _redisServiceCache = (IRedisStorageService) modelTypeContext.StorageService;

            var hashKey = writeBatchRequest.TableName;
            var writeTasks = new Dictionary<WriteRequest, Task<bool>>(writeBatchRequest.Count);

            InitWriteTasks(writeBatchRequest, hashKey, writeTasks);

            foreach (var (request, task) in writeTasks)
            {
                try
                {
                    await task.WaitCompletedSuccessfully();

                    _ = request.Response.TrySetResult(WriteResponse.GetCreatedInstance(request));
                }
                catch (Exception e)
                {
                    _ = request.Response.TrySetResult(WriteResponse.GetErrorInstance(request, e));
                }
            }

            await Task.Delay(TimeSpan.FromMilliseconds(DelayInMilliseconds), cancellationToken);
        }

        private void InitWriteTasks(IWriteBatchRequest writeBatchRequest, string? hashKey,
            IDictionary<WriteRequest, Task<bool>> writeTasks)
        {
            var requests = writeBatchRequest.ToArray();
            
            for (var index = 0; index < requests.Length; index++)
            {
                var storageItem = requests[index].ToStorageItem();

                var key = (RedisValue) storageItem.Key.Value;
                var payload = (RedisValue) storageItem.Payload;

                var writeTask = _redisServiceCache?.Writer.HashSetAsync(hashKey, key, payload, When.Always,
                    CommandFlags.FireAndForget);

                if (writeTask != null) writeTasks.Add(requests[index], writeTask);
            }
        }
    }
}