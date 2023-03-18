namespace Rydo.Storage.Redis
{
    using System;
    using StackExchange.Redis;

    internal sealed class RedisStorageService : IRedisStorageService
    {
        private readonly RedisConfiguration _redisConfiguration;
        private readonly IConnectionMultiplexer _writerMultiplexer;
        private readonly IConnectionMultiplexer _readerMultiplexer;

        public RedisStorageService(RedisConfiguration redisConfiguration)
        {
            _redisConfiguration = redisConfiguration;
            _writerMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration.WriteEndpoint);
            _readerMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration.ReadeEndpoint);
        }

        private IDatabase? _writer;

        public IDatabase Writer =>
            _writer ??= _writerMultiplexer.GetDatabase(Convert.ToInt32(_redisConfiguration.DbInstance));

        private IDatabase? _reader;

        public IDatabase Reader =>
            _reader ??= _readerMultiplexer.GetDatabase(Convert.ToInt32(_redisConfiguration.DbInstance));
    }
}