namespace Rydo.Storage.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Read;
    using StackExchange.Redis;
    using Storage.Extensions;

    internal interface IRedisReadStorageContentProvider
    {
        Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);
    }

    internal sealed class RedisReadStorageContentProvider : IRedisReadStorageContentProvider
    {
        private readonly ILogger<RedisReadStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;
        private IRedisStorageServiceService _redisServiceCache;

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

            _redisServiceCache = (IRedisStorageServiceService) modelTypeContext.StorageService;

            var readTasks = InitReadTasks(batch, batch.ModeTypeName!);
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
        }

        private static ReadResponse CreateReadResponse(RedisValue redisValue, ReadRequest readRequest)
        {
            if (!redisValue.HasValue || redisValue.IsNullOrEmpty)
                return ReadResponse.GetResponseNotFound(readRequest);

            return ReadResponse.GetResponseOk(readRequest, (byte[]) redisValue.Box());
        }

        private IDictionary<ReadRequest, Task<RedisValue>> InitReadTasks(ReadBatchRequest batch, string modelTypeName)
        {
            var readTasks = new Dictionary<ReadRequest, Task<RedisValue>>(batch.Count);
            foreach (var readRequest in batch)
            {
                var storageItemKey = readRequest.ToStorageItemKey();
                var query = (RedisValue) storageItemKey.Value;

                var task = _redisServiceCache.Reader.HashGetAsync(modelTypeName, query);
                readTasks.Add(readRequest, task);
            }

            return readTasks;
        }
    }
}