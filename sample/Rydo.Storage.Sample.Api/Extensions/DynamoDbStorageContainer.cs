namespace Rydo.Storage.Sample.Api.Extensions
{
    using Core.Models;
    using DynamoDB.Extensions;
    using Storage.Extensions;

    internal static class DynamoDbStorageContainer
    {
        public static IServiceCollection AddDynamoDbStorage(this IServiceCollection services)
        {
            var ttl = TimeSpan.FromSeconds(2);

            services.AddStorage(configurator =>
            {
                configurator.UseDynamoDb(dynamoDb =>
                {
                    dynamoDb.SetAccessKey("AKIATZMKPAMLIAH6UDOC");
                    dynamoDb.SetSecreteKey("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");
                    
                    dynamoDb.SetReadBufferSize(100);
                    dynamoDb.SetWriteBufferSize(100);

                    dynamoDb.TryAddModelType<Customer>(definition => definition.TimeToLive(ttl));
                    dynamoDb.TryAddModelType<CustomerPosition>(definition => definition.TimeToLive(ttl));
                    dynamoDb.TryAddModelType<CustomerPositionConsolidated>(definition => definition.TimeToLive(ttl));
                });
            });
            return services;
        }
    }
}