namespace Rydo.Storage.DynamoDB.Extensions
{
    using System;
    using Attributes;
    using Providers;
    using Storage.Extensions;

    public sealed class DynamoDbStorageConfiguratorBuilder :
        StorageConfiguratorBuilder<DynamoDbModelTypeDefinitionBuilder>
    {
        private string _accessKey;
        private string _secretKey;

        internal DynamoDbStorageConfiguratorBuilder()
            : base("dynamodb-storage")
        {
            _accessKey = string.Empty;
            _secretKey = string.Empty;
        }

        public DynamoDbStorageConfiguratorBuilder SetAccessKey(string accessKey)
        {
            _accessKey = accessKey;
            return this;
        }

        public DynamoDbStorageConfiguratorBuilder SetSecreteKey(string secretKey)
        {
            _secretKey = secretKey;
            return this;
        }

        public override void TryAddModelType<T>(Action<DynamoDbModelTypeDefinitionBuilder> definition = default)
        {
            ModelExtensions.TryExtractTableName<T>(out var tableName);
            
            var builder = new DynamoDbModelTypeDefinitionBuilder(typeof(T), tableName);
            definition!.Invoke(builder);
            
            var modelTypeDefinition = builder
                .ReadBufferSize(GetReadBufferSize)
                .WriteBufferSize(GetWriteBufferSize)
                .SetAccessKey(_accessKey)
                .SetSecretKey(_secretKey)
                .UseMemoryCache(UseMemoryCache)
                .Build();

            builder.Validate().ThrowIfContainsFailure();

            if (!Entries.TryGetValue(modelTypeDefinition.ModeTypeName, out _))
                Entries = Entries.Add(modelTypeDefinition.ModeTypeName, modelTypeDefinition);
        }
    }
}