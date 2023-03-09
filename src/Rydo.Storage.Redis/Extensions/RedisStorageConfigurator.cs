namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Rydo.Storage.Extensions;
    using Redis;

    public static class RedisStorageConfigurator
    {
        public static void UseRedis(this StorageContainerConfiguratorBuilder configurator,
            Action<RedisStorageConfiguratorBuilder> configure)
        {
            var builder = new RedisStorageConfiguratorBuilder();
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

                    var modelTypeContext =
                        new ModelTypeContext(modelTypeDefinition, new RedisStorageServiceService(redisConfiguration));

                    modelTypeContainer.AddModelType(modelTypeContext);
                }

                return modelTypeContainer;
            });

            configurator.Services.AddSingleton<IStorageConfiguratorBuilder>(builder);
            configurator.Services.AddSingleton<IStorageConfiguratorBuilder<RedisModelTypeDefinitionBuilder>>(builder);
            configurator.Services.AddSingleton<IStorageContentProvider, RedisStorageContentProvider>();
            configurator.Services.AddSingleton<IRedisReadStorageContentProvider, RedisReadStorageContentProvider>();
            configurator.Services.AddSingleton<IRedisWriteStorageContentProvider, RedisWriteStorageContentProvider>();
        }
    }
}