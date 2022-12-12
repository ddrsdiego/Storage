namespace Rydo.Storage.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CSharpFunctionalExtensions;

    [Serializable]
    public class ConfigurationException :
        Exception
    {
        public ConfigurationException()
        {
        }

        public ConfigurationException(IEnumerable<Result> results, string message)
            : base(message)
        {
            Results = results;
        }

        public ConfigurationException(IEnumerable<Result> results, string message, Exception innerException)
            : base(message, innerException)
        {
            Results = results;
        }

        public ConfigurationException(string message)
            : base(message)
        {
        }

        public ConfigurationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ConfigurationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IEnumerable<Result> Results { get; protected set; } = Array.Empty<Result>();
    }
}