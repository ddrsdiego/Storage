namespace Rydo.Storage.Write.Async
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.Extensions.Logging;
    using Providers;

    internal sealed class StorageClientWriterAsync : IStorageClientWriterAsync
    {
        private readonly ILogger<StorageClientWriterAsync> _logger;
        private readonly IStorageWriterConsumer _storageWriterConsumer;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public StorageClientWriterAsync(ILogger<StorageClientWriterAsync> logger,
            IStorageWriterConsumer storageWriterConsumer, IModelTypeContextContainer modelTypeContextContainer)
        {
            _logger = logger;
            _storageWriterConsumer = storageWriterConsumer;
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public ValueTask UpsertAsync<T>(string? key, T payload, FutureWriteResponse futureWriteResponse)
        {
            return InternalUpsertAsync(key, string.Empty, payload, futureWriteResponse);
        }

        public ValueTask UpsertAsync<T>(string? key, string sortKey, T payload, FutureWriteResponse futureWriteResponse)
        {
            return InternalUpsertAsync(key, sortKey, payload, futureWriteResponse);
        }

        private ValueTask InternalUpsertAsync<T>(string? key, string sortKey, T payload,
            FutureWriteResponse futureWriteResponse)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            var modelName = ModelTypeDefinitionHelper.SanitizeModeTypeName(payload.GetType());

            if (!_modelTypeContextContainer.TryGetModel(modelName, out var modelTypeContext))
                throw new Exception();

            var payloadAsUtf8 = JsonSerializer.SerializeToUtf8Bytes(payload);

            var writeRequest = WriteRequest
                .Builder(WriteRequestOperation.Upsert, futureWriteResponse)
                .WithKey(key)
                .WithSortKey(sortKey)
                .WithModelTypeDefinition(modelTypeContext.Definition)
                .WithPayload(payloadAsUtf8)
                .Build();

            var enqueueTask = _storageWriterConsumer.EnqueueRequest(writeRequest);

            return enqueueTask.IsCompletedSuccessfully
                ? new ValueTask(Task.CompletedTask)
                : SlowEnqueue(enqueueTask);

            static async ValueTask SlowEnqueue(ValueTask valueTask) => await valueTask;
        }
    }
}