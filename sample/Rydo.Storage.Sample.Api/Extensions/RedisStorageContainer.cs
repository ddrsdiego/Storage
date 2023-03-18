namespace Rydo.Storage.Sample.Api.Extensions
{
    using Core.Models;
    using Redis.Extensions;
    using Storage.Extensions;

    internal static class RedisStorageContainer
    {
        public static IServiceCollection AddRedisStorage(this IServiceCollection services)
        {
            services.AddStorage(configurator =>
            {
                configurator.UseRedis(redis =>
                {
                    redis.SetReadBufferSize(1_000);
                    redis.SetWriteBufferSize(1_000);
                    
                    redis.SetReadEndpoint("localhost:6379");
                    redis.SetWriteEndpoint("localhost:6379");

                    redis.TryAddModelType<CustomerPositionConsolidated>(definition =>
                    {
                        definition.DbInstance("1");
                        definition.TimeToLive(TimeSpan.FromSeconds(2));
                    });

                    redis.TryAddModelType<Customer>(definition =>
                    {
                        definition.DbInstance("1");
                        definition.TimeToLive(TimeSpan.FromSeconds(2));
                    });

                    redis.TryAddModelType<CustomerPosition>(definition =>
                    {
                        definition.DbInstance("1");
                        definition.TimeToLive(TimeSpan.FromSeconds(10));
                    });
                });
            });

            return services;
        }
    }
}