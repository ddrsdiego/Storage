namespace Rydo.Storage.UnitTest.Memory
{
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.DependencyInjection;
    using Storage.Memory;
    using Xunit;

    public class StorageMemoryTest
    {
        private readonly IStorageMemory? _storageMemory;
        
        public StorageMemoryTest()
        {
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddMemoryCache();
            
            services.AddSingleton<IStorageMemory, StorageMemory>();
            services.AddSingleton<IStorageMemoryRead, StorageMemoryRead>();
            services.AddSingleton<IStorageMemoryWrite, StorageMemoryWrite>();
            
            var serviceProvider = services.BuildServiceProvider();
            _storageMemory = serviceProvider.GetRequiredService<IStorageMemory>();
        }
        
        [Fact]
        public async Task Should_Validate_StorageMemoryWrite_Was_Injected()
        {
            _storageMemory?.Writer.Should().NotBeNull();
            await Task.CompletedTask;
        }
        
        [Fact]
        public async Task Should_Validate_StorageMemoryWrite_Was_Injected_1()
        {
            _storageMemory?.Writer.Should().NotBeNull();
            
            
            
            // _storageMemory?.Writer.Upsert()
            
            await Task.CompletedTask;
        }
    }
}