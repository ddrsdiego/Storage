namespace Rydo.Storage.Sample.Api.Extensions
{
    using Core.Models;
    using MongoDb.Extensions;
    using Storage.Extensions;

    public static class MongoStorageContainer
    {
        public static void AddMongoStorage(this IServiceCollection services)
        {
            services.AddStorage(configurator =>
            {
                configurator.UseMongoDb(mongo =>
                {
                    mongo.SetReadBufferSize(1_000);
                    mongo.SetWriteBufferSize(1_000);
                    mongo.ConnectionString("mongodb://localhost:27017");
                    mongo.TryAddModelType<Customer>();
                    mongo.TryAddModelType<CustomerPosition>();
                    mongo.TryAddModelType<CustomerPositionConsolidated>();
                });
            });
        }
    }
}