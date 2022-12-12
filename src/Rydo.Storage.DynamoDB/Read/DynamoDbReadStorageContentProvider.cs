namespace Rydo.Storage.DynamoDB.Read
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.DynamoDBv2.Model;
    using Extensions;
    using Internals.Metrics;
    using Microsoft.Extensions.Logging;
    using Rydo.Storage.Extensions;
    using Rydo.Storage.Read;

    public interface IDynamoDbReadStorageContentProvider
    {
        Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);
    }

    internal sealed class DynamoDbReadStorageContentProvider : IDynamoDbReadStorageContentProvider
    {
        private const int TimeLimitDefault = 100;
        private readonly ILogger<DynamoDbReadStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;
        private IDynamoDbStorageService _storageService;

        public DynamoDbReadStorageContentProvider(ILogger<DynamoDbReadStorageContentProvider> logger,
            IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger;
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();

            if (!_modelTypeContextContainer.TryGetModel(batch.ModeTypeName!, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _storageService = (IDynamoDbStorageService) modelTypeContext.StorageService;

            var tableName = modelTypeContext.Definition.TableName;

            var tasks = new Dictionary<ReadRequest, Task<GetItemResponse>>(batch.Count);
            foreach (var readRequest in batch)
            {
                var request = readRequest.ToGetItemRequest(tableName);
                tasks.Add(readRequest, _storageService.DynamoDb.GetItemAsync(request, cancellationToken));
            }

            foreach (var (readRequest, task) in tasks)
            {
                try
                {
                    var response = await task.WaitCompletedSuccessfully();

                    var readResponse = response.CreateReadResponse(readRequest);

                    _ = readRequest.Response.TrySetResult(readResponse);

                    if (readResponse.StatusCode != ReadResponseStatus.NotFound)
                        continue;

                    readRequest.PushMetricForKeyNotFound(tableName);
                }
                catch (Exception e)
                {
                    _ = readRequest.Response.TrySetResult(ReadResponse.GetResponseError(readRequest, e));
                }
            }

            sw.Stop();

            if (sw.ElapsedMilliseconds > TimeLimitDefault)
                PrometheusMetrics.ReadRequestElapsedTimeExceeded(tableName, TimeLimitDefault);
        }
    }
}