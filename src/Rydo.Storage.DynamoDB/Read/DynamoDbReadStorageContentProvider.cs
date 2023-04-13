namespace Rydo.Storage.DynamoDB.Read
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.DynamoDBv2.Model;
    using Extensions;
    using Microsoft.Extensions.Logging;
    using Rydo.Storage.Extensions;
    using Rydo.Storage.Read;

    internal sealed class DynamoDbReadStorageContentProvider : DbReadStorageContentProvider
    {
        private IDynamoDbStorageService _storageService;

        public DynamoDbReadStorageContentProvider(ILogger<DynamoDbReadStorageContentProvider> logger,
            IModelTypeContextContainer modelTypeContextContainer)
            : base(modelTypeContextContainer)
        {
        }

        public override async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();

            if (!ModelTypeContextContainer.TryGetModel(batch.ModeTypeName, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _storageService = (IDynamoDbStorageService) modelTypeContext.StorageService;

            var tableName = modelTypeContext.Definition.TableName;

            await Observable.PreExecuteRead(batch);
            
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
            
            await Observable.PostExecuteRead(batch);
        }
    }
}