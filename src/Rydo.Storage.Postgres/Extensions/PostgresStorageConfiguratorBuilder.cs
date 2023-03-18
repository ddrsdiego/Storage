namespace Rydo.Storage.Postgres.Extensions
{
    using System;
    using Providers;
    using Attributes;
    using Storage.Extensions;

    public sealed class PostgresStorageConfiguratorBuilder :
        StorageConfiguratorBuilder<PostgresModelTypeDefinitionBuilder>
    {
        private string _readEndpoint;
        private string _writeEndpoint;
        
        internal PostgresStorageConfiguratorBuilder()
            : base("postgres-storage")
        {
            _readEndpoint = string.Empty;
            _writeEndpoint = string.Empty;
        }

        public PostgresStorageConfiguratorBuilder SetReadEndpoint(string readEndpoint)
        {
            _readEndpoint = readEndpoint;
            return this;
        }

        public PostgresStorageConfiguratorBuilder SetWriteEndpoint(string writeEndpoint)
        {
            _writeEndpoint = writeEndpoint;
            return this;
        }
        
        public override void TryAddModelType<T>(Action<PostgresModelTypeDefinitionBuilder>? definition = default)
        {
            ModelExtensions.TryExtractTableName<T>(out var tableName);
            var builder = new PostgresModelTypeDefinitionBuilder(typeof(T), tableName);

            var modelTypeDefinition = builder
                .ReadEndpoint(_readEndpoint)
                .WriteEndpoint(_writeEndpoint)
                .Build();
            
            builder.Validate().ThrowIfContainsFailure();

            if (!Entries.TryGetValue(modelTypeDefinition.ModeTypeName, out _))
                Entries = Entries.Add(modelTypeDefinition.ModeTypeName, modelTypeDefinition);
        }
    }
}