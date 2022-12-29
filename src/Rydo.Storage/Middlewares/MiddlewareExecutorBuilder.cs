namespace Rydo.Storage.Middlewares
{
    using System;
    using Microsoft.Extensions.Logging;

    internal sealed class MiddlewareExecutorBuilder
    {
        private ILogger<MiddlewareExecutor> _logger;
        private IMiddlewareConfigurationContainer _middlewareConfigContainer;

        public MiddlewareExecutorBuilder WithLogger(ILogger<MiddlewareExecutor> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            return this;
        }
        
        public MiddlewareExecutorBuilder WithMiddlewareConfigContainer(
            IMiddlewareConfigurationContainer middlewareConfigContainer)
        {
            _middlewareConfigContainer = middlewareConfigContainer ??
                                         throw new ArgumentNullException(nameof(middlewareConfigContainer));
            return this;
        }

        public IMiddlewareExecutor Build()
        {
            return new MiddlewareExecutor(_logger, _middlewareConfigContainer);
        }
    }
}