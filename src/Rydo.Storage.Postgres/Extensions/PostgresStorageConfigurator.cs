namespace Rydo.Storage.Postgres.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Redis;
    using Storage.Extensions;

    public static class PostgresStorageConfigurator
    {
        public static void UsePostgres(this StorageContainerConfiguratorBuilder configurator,
            Action<PostgresStorageConfiguratorBuilder> configure)
        {
            var builder = new PostgresStorageConfiguratorBuilder();
            configure(builder);
            
            configurator.Services.AddSingleton<IModelTypeContextContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ModelTypeContextContainer>>();

                var modelTypeContainer = new ModelTypeContextContainer(logger);

                foreach (var (_, modelTypeDefinition) in builder.Entries)
                {
                    var redisConfiguration = new RedisConfiguration
                    {
                        DbInstance = modelTypeDefinition.DbInstance,
                        ReadeEndpoint = modelTypeDefinition.ReadEndpoint,
                        WriteEndpoint = modelTypeDefinition.WriteEndpoint
                    };

                    // var modelTypeContext =
                    //     new ModelTypeContext(modelTypeDefinition, new RedisStorageServiceService(redisConfiguration));
                    //
                    // modelTypeContainer.AddModelType(modelTypeContext);
                }

                return modelTypeContainer;
            });
        }
    }
}