namespace Rydo.Storage.Sample.Api
{
    using System.Reflection;
    using Core.Domain.CustomerAggregate;
    using Extensions;
    using Microsoft.OpenApi.Models;
    using Serilog;
    using Serilog.Events;
    using Serilog.Exceptions;
    using Serilog.Exceptions.Core;
    using Serilog.Formatting.Elasticsearch;
    using Serilog.Sinks.SystemConsole.Themes;

    public static class Program
    {
        private const string EnvironmentProduction = "Development";
        private const string AspnetcoreEnvironment = "ASPNETCORE_ENVIRONMENT";
        private static string Env => Environment.GetEnvironmentVariable(AspnetcoreEnvironment) ?? EnvironmentProduction;

        public static async Task Main(string[] args) => await CreateHostBuilder(args).Build().RunAsync();

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog(UserCustomSerilog())
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureServices((context, services) =>
                {
                    services.AddControllers();
                    services.AddRedisStorage();
                    services.AddSingleton<ICustomerRepository, CustomerRepository>();
                    services.AddSwaggerGen(c =>
                        c.SwaggerDoc("v1", new OpenApiInfo {Title = Assembly.GetEntryAssembly()?.GetName().Name}));
                });

        private static Action<HostBuilderContext, IServiceProvider, LoggerConfiguration> UserCustomSerilog()
        {
            return (context, provider, logger) =>
            {
                var assembly = Assembly.GetExecutingAssembly().GetName();
                var isDebug = context.Configuration.GetSection("IsDebug").Get<bool>();
                    
                logger
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext()
                    .Destructure.ToMaximumCollectionCount(10)
                    .Destructure.ToMaximumStringLength(1024)
                    .Destructure.ToMaximumDepth(5)
                    .Enrich.WithProperty("Jornada", "Foundations")
                    .Enrich.WithProperty("Assembly", $"{assembly.Name}")
                    .Enrich.WithProperty("Version", $"{assembly.Version}")
                    .Enrich
                    .WithExceptionDetails(new DestructuringOptionsBuilder()
                        .WithDefaultDestructurers()
                        .WithRootName("Exception"));
                    
                if (isDebug)
                {
                    logger.WriteTo.Async(sinkConfigurations =>
                        sinkConfigurations.Console(
                            outputTemplate:
                            "{Timestamp:HH:mm:ss} {Level:u3} => {Message:lj}{Properties:j}{NewLine}{Exception}",
                            restrictedToMinimumLevel: LogEventLevel.Debug, theme: AnsiConsoleTheme.Code));
                }
                else
                {
                    logger.WriteTo.Async(sinkConfigurations =>
                        sinkConfigurations.Console(new ElasticsearchJsonFormatter(inlineFields: true,
                            renderMessageTemplate: false)));
                }
            };
        }
    }
}