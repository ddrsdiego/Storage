namespace Rydo.Storage.Read
{
    using System;
    using System.Threading.Tasks;
    using Extensions;
    using Providers;

    internal sealed class StorageClientReader : IStorageClientReader
    {
        private readonly IStorageReaderConsumer _storageReaderConsumer;
        private readonly IModelTypeContextContainer _modelTypeContextContainer;

        public StorageClientReader(IStorageReaderConsumer storageReaderConsumer,
            IModelTypeContextContainer modelTypeContextContainer)
        {
            _storageReaderConsumer = storageReaderConsumer;
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public async ValueTask<ReadResponse> Read<T>(string key)
        {
            if (key == null || string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var futureResponse = FutureReadResponse.GetInstance();

            await InternalReadAsync<T>(key, string.Empty, futureResponse);

            return await futureResponse.ReadTask;
        }

        public async ValueTask<ReadResponse> Read<T>(string key, string sortKey)
        {
            if (key == null || string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            var futureResponse = FutureReadResponse.GetInstance();

            await InternalReadAsync<T>(key, sortKey, futureResponse);

            return await futureResponse.ReadTask;
        }

        public ValueTask ReadAsync<T>(string key, FutureReadResponse futureReadResponse) =>
            InternalReadAsync<T>(key, string.Empty, futureReadResponse);

        public ValueTask ReadAsync<T>(string key, string sortKey, FutureReadResponse futureReadResponse) =>
            InternalReadAsync<T>(key, sortKey, futureReadResponse);

        private ValueTask InternalReadAsync<T>(string key, string sortKey, FutureReadResponse futureReadResponse)
        {
            var modelName = ModelTypeDefinitionHelper.SanitizeModeTypeName(typeof(T));

            if (!_modelTypeContextContainer.TryGetModel(modelName, out var modelTypeContext))
                throw new Exception();

            var request = new ReadRequest(key, sortKey, modelTypeContext.Definition, futureReadResponse);

            var enqueueTask = _storageReaderConsumer.EnqueueRequest(request);

            return enqueueTask.IsCompletedSuccessfully
                ? new ValueTask(Task.CompletedTask)
                : SlowEnqueue(enqueueTask);

            static async ValueTask SlowEnqueue(ValueTask valueTask) => await valueTask;
        }
    }
}