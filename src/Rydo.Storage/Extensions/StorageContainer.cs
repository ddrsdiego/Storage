namespace Rydo.Storage.Extensions
{
    using System;
    using Attributes;
    using Memory;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Read;
    using Serialization;
    using Write;
    using Write.Async;
    using Write.Sync;

    public static class StorageContainer
    {
        public static IServiceCollection AddStorage(this IServiceCollection services,
            Action<StorageContainerConfiguratorBuilder> configurator)
        {
            var storageContainerConfiguratorBuilder = new StorageContainerConfiguratorBuilder(services);
            configurator(storageContainerConfiguratorBuilder);

            services.AddMemoryCache();
            
            services.AddSingleton<IStorageMemoryRead, StorageMemoryRead>();
            services.AddSingleton<IStorageMemoryWrite, StorageMemoryWrite>();
            
            services.AddSingleton<IStorageMemory>(sp =>
            {
                var logger = sp.GetService<ILogger<StorageMemory>>();
                
                var memoryCache = sp.GetService<IMemoryCache>();
                var memoryRead = sp.GetService<IStorageMemoryRead>();
                var memoryWrite = sp.GetService<IStorageMemoryWrite>();
                return new StorageMemory(logger, memoryCache, memoryWrite, memoryRead);
            });

            services.AddSingleton<IStorageClient, StorageClient>();
            services.AddSingleton<ITableStorageManager, TableStorageManager>();
            services.TryAddSingleton<IRydoStorageCacheSerializer>(_ => new RydoStorageCacheSystemTextJsonSerializer());

            services.AddReadServices();
            services.AddWriteServices();

            return services;
        }

        private static void AddReadServices(this IServiceCollection services)
        {
            services.AddSingleton<IStorageRead, InternalStorageRead>();
            services.AddSingleton<IStorageClientReader, StorageClientReader>();
            services.AddSingleton<IStorageReaderConsumer, StorageReaderConsumer>();
        }

        private static void AddWriteServices(this IServiceCollection services)
        {
            services.AddSingleton<IStorageWrite, InternalStorageWrite>();
            services.AddSingleton<IStorageClientWrite, StorageClientWrite>();
            services.AddSingleton<IStorageWriterConsumer, StorageWriterConsumer>();
            services.AddSingleton<IStorageClientWriteSync, StorageClientWriteSync>();
            services.AddSingleton<IStorageClientWriterAsync, StorageClientWriterAsync>();
        }
    }
}