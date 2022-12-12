namespace Rydo.Storage.MongoDb.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Storage.Extensions;

    public static class MongoDbStorageConfigurator
    {
        public static void UseMongoDb(this StorageContainerConfiguratorBuilder configurator,
            Action<MongoDbStorageConfiguratorBuilder> configure)
        {
            var builder = new MongoDbStorageConfiguratorBuilder();
            configure(builder);

            configurator.Services.AddSingleton<IModelTypeContextContainer>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<ModelTypeContextContainer>>();
                var modelTypeContainer = new ModelTypeContextContainer(logger);
                return modelTypeContainer;
            });
        }
    }
}