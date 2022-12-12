namespace Rydo.Storage.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Write;
    using StackExchange.Redis;
    using Storage.Extensions;

    internal interface IRedisWriteStorageContentProvider
    {
        Task Upsert(IWriteBatchRequest writeBatchRequest);
    }

    internal sealed class RedisWriteStorageContentProvider : IRedisWriteStorageContentProvider
    {
        private const int DelayInMilliseconds = 1;
        private IRedisStorageServiceService? _redisServiceCache;
        private readonly ILogger<RedisWriteStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public RedisWriteStorageContentProvider(ILogger<RedisWriteStorageContentProvider> logger,
            IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger;
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public async Task Upsert(IWriteBatchRequest writeBatchRequest)
        {
            if (!_modelTypeContextContainer.TryGetModel(writeBatchRequest.ModeTypeName!, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _redisServiceCache = (IRedisStorageServiceService) modelTypeContext.StorageService;

            var hashKey = writeBatchRequest.ModeTypeName;
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

            await Task.Delay(TimeSpan.FromMilliseconds(DelayInMilliseconds));
        }

        private void InitWriteTasks(IWriteBatchRequest writeBatchRequest, string? hashKey,
            IDictionary<WriteRequest, Task<bool>> writeTasks)
        {
            foreach (var writeRequest in writeBatchRequest)
            {
                var storageItem = writeRequest.ToStorageItem();

                var key = (RedisValue) storageItem.Key.Value;
                var payload = (RedisValue) storageItem.Payload;

                var writeTask = _redisServiceCache!.Writer.HashSetAsync(hashKey, key, payload, When.Always,
                    CommandFlags.FireAndForget);

                writeTasks.Add(writeRequest, writeTask);
            }
        }
    }
}