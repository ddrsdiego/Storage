namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Providers;
    using Storage.Extensions;

    internal static class ModelTypeContextContainerResolverEx
    {
        public static IModelTypeContextContainer TryResolveModelTypeContextContainer(
            this StorageContainerConfiguratorBuilder configurator, IServiceProvider sp)
        {
            var logger = sp.GetRequiredService<ILogger<ModelTypeContextContainer>>();
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

                modelTypeContainer.AddModelType(modelTypeContext);
            }

            return modelTypeContainer;
        }
    }
}