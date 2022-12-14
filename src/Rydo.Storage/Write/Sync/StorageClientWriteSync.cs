namespace Rydo.Storage.Write.Sync
{
    using System;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Attributes;
    using Extensions;
    using Microsoft.Extensions.Logging;
    using Providers;

    internal sealed class StorageClientWriteSync : IStorageClientWriteSync
    {
        private readonly ILogger<StorageClientWriteSync> _logger;
        private readonly IStorageWriterConsumer _storageWriterConsumer;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;
        private readonly ITableStorageManager _tableStorageManager;

        public StorageClientWriteSync(ILogger<StorageClientWriteSync> logger,
            IStorageWriterConsumer storageWriterConsumer, IModelTypeContextContainer modelTypeContextContainer,
            ITableStorageManager tableStorageManager)
        {
            _logger = logger;
            _storageWriterConsumer = storageWriterConsumer;
            _modelTypeContextContainer = modelTypeContextContainer;
            _tableStorageManager = tableStorageManager;
        }

        public ValueTask<WriteResponse> Upsert<T>(string key, T payload) => Upsert(key, string.Empty, payload);

        public async ValueTask<WriteResponse> Upsert<T>(string key, string sortKey, T payload)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            var modelName = ModelTypeDefinitionHelper.SanitizeModeTypeName(payload.GetType());

            if (!_modelTypeContextContainer.TryGetModel(modelName, out var modelTypeContext))
                throw new Exception();

            var future = FutureWriteResponse.GetInstance();
            var payloadAsUtf8 = JsonSerializer.SerializeToUtf8Bytes(payload);

            var writeRequest = WriteRequest
                .Builder(WriteRequestOperation.Upsert, future)
                .WithModelTypeDefinition(modelTypeContext.Definition)
                .WithKey(key)
                .WithSortKey(sortKey)
                .WithPayload(payloadAsUtf8)
                .Build();

            _ = _storageWriterConsumer.EnqueueRequest(writeRequest);

            return await writeRequest.Response.WriteTask;
        }

        public ValueTask<WriteResponse> Remove<T>(string key) => Remove<T>(key, string.Empty);

        public async ValueTask<WriteResponse> Remove<T>(string key, string sortKey)
        {
            var modelName = ModelTypeDefinitionHelper.SanitizeModeTypeName(typeof(T));

            if (!_modelTypeContextContainer.TryGetModel(modelName, out var modelTypeContext))
                throw new Exception();

            var future = FutureWriteResponse.GetInstance();
            var writeRequest = WriteRequest.Builder(WriteRequestOperation.Remove, future)
                .WithModelTypeDefinition(modelTypeContext.Definition)
                .WithKey(key)
                .WithSortKey(sortKey)
                .Build();

            _ = _storageWriterConsumer.EnqueueRequest(writeRequest);

            return await writeRequest.Response.WriteTask;
        }
    }
}