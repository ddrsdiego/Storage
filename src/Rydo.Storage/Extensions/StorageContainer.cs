namespace Rydo.Storage.Extensions
{
    using System;
    using Attributes;
    using Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Read;
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

            services.AddSingleton<IStorageMemory, StorageMemory>();
            services.AddSingleton<IStorageClient, StorageClient>();
            services.AddSingleton<ITableStorageManager, TableStorageManager>();
            
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