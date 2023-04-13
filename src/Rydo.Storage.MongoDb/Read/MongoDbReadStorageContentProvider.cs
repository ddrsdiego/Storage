namespace Rydo.Storage.MongoDb.Read
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Rydo.Storage.Extensions;
    using Rydo.Storage.Read;

    internal sealed class MongoDbReadStorageContentProvider : DbReadStorageContentProvider
    {
        private IMongoDbStorageService _storageService;
        private readonly ILogger<MongoDbReadStorageContentProvider> _logger;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public MongoDbReadStorageContentProvider(IModelTypeContextContainer modelTypeContextContainer) 
            : base(modelTypeContextContainer)
        {
        }

        public override async Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            if (!_modelTypeContextContainer.TryGetModel(batch.ModeTypeName, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _storageService = (IMongoDbStorageService) modelTypeContext.StorageService;

            await Task.Delay(TimeSpan.FromMilliseconds(1), cancellationToken);
        }
    }
}