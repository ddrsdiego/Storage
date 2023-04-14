namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Storage.Extensions;
    using Write;

    internal static class DbWriteStorageContentProviderResolverEx
    {
        public static IDbWriteStorageContentProvider TryResolveRedisWriteStorageContentProvider(
            this StorageContainerConfiguratorBuilder configurator, IServiceProvider sp)
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var modelTypeContextContainer = sp.GetRequiredService<IModelTypeContextContainer>();

            var redisWriteStorageContentProvider = new RedisWriteStorageContentProvider(modelTypeContextContainer);

            return redisWriteStorageContentProvider;
        }
    }
}