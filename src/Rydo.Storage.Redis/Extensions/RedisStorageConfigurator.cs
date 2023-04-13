namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Read;
    using Read.Observers;
    using Rydo.Storage.Extensions;
    using Redis;
    using Write;

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
                        new ModelTypeContext(modelTypeDefinition, new RedisStorageService(redisConfiguration));

                    modelTypeContainer.AddModelType(modelTypeContext);
                }

                return modelTypeContainer;
            });

            configurator.Services.AddSingleton<IStorageConfiguratorBuilder>(builder);
            configurator.Services.AddSingleton<IStorageConfiguratorBuilder<RedisModelTypeDefinitionBuilder>>(builder);
            configurator.Services.AddSingleton<IStorageContentProvider, RedisStorageContentProvider>();
            configurator.Services.AddSingleton<IDbReadStorageContentProvider>(sp =>
            {
                var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
                var modelTypeContextContainer = sp.GetRequiredService<IModelTypeContextContainer>();
                var redisReadStorageContentProvider = new RedisReadStorageContentProvider(modelTypeContextContainer);
                redisReadStorageContentProvider.ConnectDbReadStorageContentProviderObserver(new LogDbReadStorageContentProviderObserver(loggerFactory));

                return redisReadStorageContentProvider;
            });
            configurator.Services.AddSingleton<IDbWriteStorageContentProvider, RedisWriteStorageContentProvider>();
        }
    }
}