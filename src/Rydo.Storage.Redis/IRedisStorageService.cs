namespace Rydo.Storage.Redis
{
    using Providers;
    using StackExchange.Redis;

    public interface IRedisStorageServiceService : IStorageService
    {
        IDatabase Reader { get; }
        IDatabase Writer { get; }
    }
}