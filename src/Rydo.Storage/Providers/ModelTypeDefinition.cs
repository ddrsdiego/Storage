namespace Rydo.Storage.Providers
{
    using System;

    public readonly struct ModelTypeContext
    {
        public ModelTypeContext(IModelTypeDefinition definition, IStorageService storageService)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            StorageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        public readonly IStorageService StorageService;
        public readonly IModelTypeDefinition Definition;
    }

    public sealed class ModelTypeDefinition : IModelTypeDefinition
    {
        internal ModelTypeDefinition(Type modelType)
        {
            ModelType = modelType;
            ModeTypeName = SanitizeModeTypeName(modelType);
        }

        private static string SanitizeModeTypeName(Type modelType) =>
            ModelTypeDefinitionHelper.SanitizeModeTypeName(modelType);

        public Type ModelType { get; }
        public string TableName { get; internal set; }
        public string ModeTypeName { get; }
        public TimeSpan TimeToLive { get; internal set; }
        public string WriteEndpoint { get; internal set; }
        public string ReadEndpoint { get; internal set; }
        public bool UseMemoryCache { get; internal set; }
        public string DbInstance { get; internal set; }
        public int WriteBufferSize { get; internal set; }
        public int ReadBufferSize { get; internal set; }
        public string AccessKey { get; internal set; }
        public string SecretKey { get; internal set; }
    }
}