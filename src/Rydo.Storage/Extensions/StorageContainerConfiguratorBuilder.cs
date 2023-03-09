namespace Rydo.Storage.Extensions
{
    using Microsoft.Extensions.DependencyInjection;

    public sealed class StorageContainerConfiguratorBuilder
    {
        public StorageContainerConfiguratorBuilder(IServiceCollection services)
        {
            Services = services;
        
        }

        public IServiceCollection Services { get; }
        
    }
}