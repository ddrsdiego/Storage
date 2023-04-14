namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
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

            configurator.Services.AddSingleton<IStorageConfiguratorBuilder>(builder);
            configurator.Services.AddSingleton<IStorageContentProvider, RedisStorageContentProvider>();
            configurator.Services.AddSingleton<IStorageConfiguratorBuilder<RedisModelTypeDefinitionBuilder>>(builder);
            
            configurator.Services.AddSingleton(configurator.TryResolveModelTypeContextContainer);
            configurator.Services.AddSingleton(configurator.TryResolveRedisReadStorageContentProvider);
            configurator.Services.AddSingleton(configurator.TryResolveRedisWriteStorageContentProvider);
        }
    }
}