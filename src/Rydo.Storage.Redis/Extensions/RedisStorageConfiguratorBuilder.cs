namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Attributes;
    using Providers;
    using Storage.Extensions;

    public sealed class RedisStorageConfiguratorBuilder :
        StorageConfiguratorBuilder<RedisModelTypeDefinitionBuilder>
    {
        private string _readEndpoint;
        private string _writeEndpoint;

        internal RedisStorageConfiguratorBuilder()
            : base("redis-storage")
        {
            _readEndpoint = string.Empty;
            _writeEndpoint = string.Empty;
        }

        public RedisStorageConfiguratorBuilder SetReadEndpoint(string readEndpoint)
        {
            _readEndpoint = readEndpoint;
            return this;
        }

        public RedisStorageConfiguratorBuilder SetWriteEndpoint(string writeEndpoint)
        {
            _writeEndpoint = writeEndpoint;
            return this;
        }

        public override void TryAddModelType<T>(Action<RedisModelTypeDefinitionBuilder>? definition = default)
        {
            ModelExtensions.TryExtractTableName<T>(out var tableName);

            var builder = new RedisModelTypeDefinitionBuilder(typeof(T), tableName);
            
            definition?.Invoke(builder);
           
            var modelTypeDefinition = builder
                .WriteBufferSize(GetWriteBufferSize)
                .ReadBufferSize(GetReadBufferSize)
                .ReadEndpoint(_readEndpoint)
                .WriteEndpoint(_writeEndpoint)
                .Build();

            builder.Validate().ThrowIfContainsFailure();
            
            if (!Entries.TryGetValue(modelTypeDefinition.ModeTypeName, out _))
                Entries = Entries.Add(modelTypeDefinition.ModeTypeName, modelTypeDefinition);
        }
    }
}