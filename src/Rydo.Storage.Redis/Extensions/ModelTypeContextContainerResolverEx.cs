namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Storage.Extensions;
    using v2;
    
    internal static class ModelTypeContextContainerResolverEx
    {
        public static IModelTypeContextContainer TryResolveModelTypeContextContainer(
            this StorageContainerConfiguratorBuilder configurator, IServiceProvider sp)
        {
            var logger = sp.GetRequiredService<ILogger<ModelTypeContextContainer>>();
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var storageMemory = sp.GetRequiredService<IStorageMemory>();
            var builder = sp.GetRequiredService<IStorageConfiguratorBuilder<RedisModelTypeDefinitionBuilder>>();

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

                var provider = new RedisStorageContentProvider(loggerFactory,
                    new RedisWriteStorageContentProvider(modelTypeContext),
                    new RedisReadStorageContentProvider(modelTypeContext));

                var writer =
                    new Write.v2.InternalStorageWrite(loggerFactory, storageMemory, provider, modelTypeContext);
                
                modelTypeContainer.AddModelType(modelTypeContext);
            }

            return modelTypeContainer;
        }
    }
}