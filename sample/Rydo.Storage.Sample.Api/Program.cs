namespace Rydo.Storage.Sample.Api
{
    using System.Reflection;
    using Core.Domain.CustomerAggregate;
    using Extensions;
    using Microsoft.OpenApi.Models;

    public static class Program
    {
        private const string EnvironmentProduction = "Development";
        private const string AspnetcoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private static string Env => Environment.GetEnvironmentVariable(AspnetcoreEnvironment) ?? EnvironmentProduction;

        public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureServices((context, services) =>
                {
                    services.AddControllers();
                    services.AddRedisStorage();
                    services.AddSingleton<ICustomerRepository, CustomerRepository>();
                    services.AddSwaggerGen(c =>
                        c.SwaggerDoc("v1", new OpenApiInfo {Title = Assembly.GetEntryAssembly()?.GetName().Name}));
                });
    }
}