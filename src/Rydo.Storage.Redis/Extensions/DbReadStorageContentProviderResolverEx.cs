namespace Rydo.Storage.Redis.Extensions
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Read;
    using Read.Observers;
    using Storage.Extensions;

    internal static class DbReadStorageContentProviderResolverEx
    {
        public static IDbReadStorageContentProvider TryResolveRedisReadStorageContentProvider(
            this StorageContainerConfiguratorBuilder configurator, IServiceProvider sp)
        {
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();
            var modelTypeContextContainer = sp.GetRequiredService<IModelTypeContextContainer>();

            var redisReadStorageContentProvider = new RedisReadStorageContentProvider(modelTypeContextContainer);

            redisReadStorageContentProvider.ConnectDbReadStorageContentProviderObserver(
                new LogDbReadStorageContentProviderObserver(loggerFactory));
            
            redisReadStorageContentProvider.ConnectDbReadStorageContentProviderObserver(
                new MetricDbReadStorageContentProviderObserver());
            
            return redisReadStorageContentProvider;
        }
    }
}