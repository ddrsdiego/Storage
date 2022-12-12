namespace Rydo.Storage.Middlewares
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;

    public interface IMiddlewareConfigurationContainer
    {
        IList<MiddlewareConfiguration> Configs { get; }
        MiddlewareConfiguration FinallyProcess { get; }

        int TotalConfigurations { get; }
    }

    public class MiddlewareConfigurationContainer : IMiddlewareConfigurationContainer
    {
        public MiddlewareConfigurationContainer(IList<MiddlewareConfiguration> configs,
            MiddlewareConfiguration finallyProcess)
        {
            Configs = configs;
            FinallyProcess = finallyProcess;
        }

        public IList<MiddlewareConfiguration> Configs { get; }
        public MiddlewareConfiguration FinallyProcess { get; }
        public int TotalConfigurations => Configs.Count + 1;
    }

    public class MiddlewareConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiddlewareConfiguration"/> class.
        /// </summary>
        /// <param name="type">The middleware type</param>
        /// <param name="lifetime"><inheritdoc cref="Lifetime"/>The middleware instance lifetime</param>
        /// <param name="instanceContainerId">The instance container ID used to get the correct container when creating the instance</param>
        public MiddlewareConfiguration(Type type, ServiceLifetime lifetime, Guid? instanceContainerId = null)
        {
            Type = type;
            Lifetime = lifetime;
            InstanceContainerId = instanceContainerId;
        }

        /// <summary>
        /// Gets the middleware type
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// Gets the middleware instance lifetime
        /// </summary>
        public readonly ServiceLifetime Lifetime;

        /// <summary>
        /// Gets the instance container ID used to get the desired factory when creating the instance
        /// </summary>
        public readonly Guid? InstanceContainerId;
    }
}