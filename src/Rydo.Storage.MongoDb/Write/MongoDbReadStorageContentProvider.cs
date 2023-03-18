namespace Rydo.Storage.MongoDb.Write
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Read;
    using Storage.Extensions;

    internal sealed class MongoDbReadStorageContentProvider : IDbReadStorageContentProvider
    {
        private IMongoDbStorageService _storageService;
        private readonly ILogger<MongoDbReadStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public MongoDbReadStorageContentProvider(ILogger<MongoDbReadStorageContentProvider> logger,
            IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger;
            _modelTypeContextContainer = modelTypeContextContainer;
        }
        
        public async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            if (!_modelTypeContextContainer.TryGetModel(batch.ModeTypeName, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");
            
            _storageService = (IMongoDbStorageService) modelTypeContext.StorageService;
            
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("[{BatchId}] - Starting query for {BatchCount} item(s) on {TableName} table", 
                batch.BatchId,
                batch.Count,
                batch.TableName);
            
            _logger.LogInformation("[{BatchId}] - Finishing query for {BatchCount} item(s) on {TableName} table, Elapsed Time: {ElapsedMilliseconds}", 
                batch.BatchId,
                batch.Count,
                batch.TableName,
                sw.ElapsedMilliseconds);
            
            await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        }
    }
}