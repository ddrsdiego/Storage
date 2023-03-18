namespace Rydo.Storage.Redis
{
    using Providers;
    using StackExchange.Redis;

    public interface IRedisStorageService : IStorageService
    {
        IDatabase Reader { get; }
        IDatabase Writer { get; }
    }
}