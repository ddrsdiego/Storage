namespace Rydo.Storage.Middlewares.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Middlewares;

    internal static class MiddlewareExtension
    {
        public static void AddMiddlewares(this IServiceCollection services)
        {
            var configs = new List<MiddlewareConfiguration>();

            configs.Insert(configs.Count,
                new MiddlewareConfiguration(typeof(SerializerConsumerMiddleware), ServiceLifetime.Scoped));

            configs.Insert(configs.Count,
                new MiddlewareConfiguration(typeof(CustomConsumerMiddleware), ServiceLifetime.Scoped));

            configs.Insert(configs.Count,
                new MiddlewareConfiguration(typeof(DeadLetterHandleMiddleware), ServiceLifetime.Scoped));

            var container = new MiddlewareConfigurationContainer(configs,
                new MiddlewareConfiguration(typeof(OffsetCommitManagerMiddleware), ServiceLifetime.Scoped));

            foreach (var middlewareConfiguration in container.Configs)
            {
                services.Add(new ServiceDescriptor(middlewareConfiguration.Type, middlewareConfiguration.Type,
                    middlewareConfiguration.Lifetime));
            }

            services.Add(new ServiceDescriptor(container.FinallyProcess.Type, container.FinallyProcess.Type,
                container.FinallyProcess.Lifetime));

            services.AddSingleton(configs);
            services.AddSingleton<IMiddlewareConfigurationContainer>(container);
        }
    }

    internal class OffsetCommitManagerMiddleware : IMessageMiddleware
    {
        public Task Invoke(MessageConsumerContext context, MiddlewareDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class DeadLetterHandleMiddleware : IMessageMiddleware
    {
        public Task Invoke(MessageConsumerContext context, MiddlewareDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class CustomConsumerMiddleware : IMessageMiddleware
    {
        public Task Invoke(MessageConsumerContext context, MiddlewareDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }

    internal class SerializerConsumerMiddleware : IMessageMiddleware
    {
        public Task Invoke(MessageConsumerContext context, MiddlewareDelegate next)
        {
            throw new System.NotImplementedException();
        }
    }
}