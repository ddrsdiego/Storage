namespace Rydo.Storage.Write
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Extensions;
    using Memory;
    using Microsoft.Extensions.Logging;
    using Providers;

    internal sealed class InternalStorageWrite : IStorageWrite
    {
        private readonly IStorageMemory _storageMemory;
        private readonly ILogger<InternalStorageWrite> _logger;
        private readonly IStorageContentProvider _storageContentProvider;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public InternalStorageWrite(ILogger<InternalStorageWrite> logger, IStorageMemory storageMemory,
            IStorageContentProvider storageContentProvider, IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storageMemory = storageMemory ?? throw new ArgumentNullException(nameof(storageMemory));
            _storageContentProvider =
                storageContentProvider ?? throw new ArgumentNullException(nameof(storageContentProvider));
            _modelTypeContextContainer = modelTypeContextContainer ??
                                         throw new ArgumentNullException(nameof(modelTypeContextContainer));
        }

        public Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            _modelTypeContextContainer.TryGetModel(writeBatchRequest.ModeTypeName!, out var modelTypeDefinition);

            if (modelTypeDefinition.Definition.UseMemoryCache)
            {
                var task = _storageMemory.Upsert(writeBatchRequest);
                var res = task.IsCompletedSuccessfully ? Task.CompletedTask : SlowWrite(task.AsTask());
            }

            var slowWrite = _storageContentProvider.Write(writeBatchRequest, cancellationToken);
            
            try
            {
                return slowWrite.IsCompletedSuccessfully ? Task.CompletedTask : SlowWrite(slowWrite);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
                throw;
            }

            static async Task SlowWrite(Task task) => await task;
        }
    }
}