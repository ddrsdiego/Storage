namespace Rydo.Storage.Postgres.Extensions
{
    using System;
    using Providers;
    using Attributes;
    using Storage.Extensions;

    public class PostgresStorageConfiguratorBuilder :
        StorageConfiguratorBuilder<PostgresModelTypeDefinitionBuilder>
    {
        public PostgresStorageConfiguratorBuilder()
            : base("postgres-storage")
        {
        }

        public override void TryAddModelType<T>(Action<PostgresModelTypeDefinitionBuilder>? definition = default)
        {
            ModelExtensions.TryExtractTableName<T>(out var tableName);
            var builder = new PostgresModelTypeDefinitionBuilder(typeof(T), tableName);

            var modelTypeDefinition = builder
                .Build();
            
            builder.Validate().ThrowIfContainsFailure();

            if (!Entries.TryGetValue(modelTypeDefinition.ModeTypeName, out _))
                Entries = Entries.Add(modelTypeDefinition.ModeTypeName, modelTypeDefinition);
        }
    }
}