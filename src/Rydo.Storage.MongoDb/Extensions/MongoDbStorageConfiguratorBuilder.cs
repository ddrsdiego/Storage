namespace Rydo.Storage.MongoDb.Extensions
{
    using System;
    using Providers;
    using Storage.Extensions;

    public sealed class MongoDbStorageConfiguratorBuilder :
        StorageConfiguratorBuilder<MongoModelTypeDefinitionBuilder>
    {
        private string _connectionString;
        private string _username;
        private string _password;

        public MongoDbStorageConfiguratorBuilder()
            : base("mongodb-storage")
        {
            _username = string.Empty;
            _password = string.Empty;
            _connectionString = string.Empty;
        }

        public MongoDbStorageConfiguratorBuilder ConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }

        public MongoDbStorageConfiguratorBuilder Username(string username)
        {
            _username = username;
            return this;
        }

        public MongoDbStorageConfiguratorBuilder Password(string password)
        {
            _password = password;
            return this;
        }

        public override void TryAddModelType<T>(Action<MongoModelTypeDefinitionBuilder> definition = default)
        {
            var builder = new MongoModelTypeDefinitionBuilder();
            definition?.Invoke(builder);

            var modelTypeDefinition = builder
                .ConnectionString(_connectionString)
                .Build();
            
            builder.Validate().ThrowIfContainsFailure();
            
            if (!Entries.TryGetValue(modelTypeDefinition.ModeTypeName, out _))
                Entries = Entries.Add(modelTypeDefinition.ModeTypeName, modelTypeDefinition);
        }
    }
}