namespace Rydo.Storage.Sample.Api.Extensions
{
    using Core.Models;
    using Postgres.Extensions;
    using Storage.Extensions;

    internal static class PostgresContainer
    {
        public static IServiceCollection AddPostgresStorage(this IServiceCollection services)
        {
            services.AddStorage(configurator =>
            {
                configurator.UsePostgres(postgres =>
                {
                    postgres.SetReadEndpoint("Server=localhost;Port=5432;Database=Rydo.Storage.Sample.Api;User Id=admin;Password=admin;");
                    postgres.SetWriteEndpoint("Server=localhost;Port=5432;Database=Rydo.Storage.Sample.Api;User Id=admin;Password=admin;");
                    
                    postgres.TryAddModelType<CustomerPositionConsolidated>(definition =>
                    {
                    });
                });
            });
            return services;
        }
    }
}