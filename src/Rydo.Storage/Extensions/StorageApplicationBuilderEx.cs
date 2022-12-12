namespace Rydo.Storage.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Read;
    using Write;

    public static class StorageApplicationBuilderEx
    {
        public static IApplicationBuilder UseStorage(this IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.ApplicationServices.GetRequiredService<IStorageWriterConsumer>();
            applicationBuilder.ApplicationServices.GetRequiredService<IStorageReaderConsumer>();
            applicationBuilder.ApplicationServices.GetRequiredService<IModelTypeContextContainer>();
            
            return applicationBuilder;
        }
    }
}