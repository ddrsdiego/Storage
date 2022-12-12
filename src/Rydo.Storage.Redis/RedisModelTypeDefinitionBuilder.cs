namespace Rydo.Storage.Redis
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Providers;

    public sealed class RedisModelTypeDefinitionBuilder : ModelTypeDefinitionBuilder
    {
        private readonly Type _modelType;
        private string _dbInstance;
        private string _writeEndpoint;
        private string _readEndpoint;
        private int _writeBufferSize;
        private int _readBufferSize;
        private TimeSpan _timeToLive;
        private bool _useMemoryCache;


        public RedisModelTypeDefinitionBuilder(Type type, string? tableName)
            : base(tableName)
        {
            _modelType = type;
            _dbInstance = "-1";
            _readEndpoint = string.Empty;
            _writeEndpoint = string.Empty;
        }

        public RedisModelTypeDefinitionBuilder WriteBufferSize(int writeBufferSize)
        {
            _writeBufferSize = writeBufferSize;
            return this;
        }

        public RedisModelTypeDefinitionBuilder ReadBufferSize(int readBufferSize)
        {
            _readBufferSize = readBufferSize;
            return this;
        }

        public RedisModelTypeDefinitionBuilder TimeToLive(TimeSpan timeToLive)
        {
            _timeToLive = timeToLive;
            return this;
        }

        public RedisModelTypeDefinitionBuilder WriteEndpoint(string writeEndpoint)
        {
            _writeEndpoint = writeEndpoint;
            return this;
        }

        public RedisModelTypeDefinitionBuilder ReadEndpoint(string readEndpoint)
        {
            _readEndpoint = readEndpoint;
            return this;
        }

        public RedisModelTypeDefinitionBuilder DbInstance(string dbInstance)
        {
            _dbInstance = dbInstance;
            return this;
        }

        public IModelTypeDefinitionBuilder UseMemoryCache()
        {
            UseMemoryCache(true);
            return this;
        }

        private void UseMemoryCache(bool useMemoryCache) => _useMemoryCache = useMemoryCache;

        public override IEnumerable<Result> Validate()
        {
            if (string.IsNullOrEmpty(_readEndpoint))
                yield return Result.Failure("The read endpoint address was not provided.");

            if (string.IsNullOrEmpty(_writeEndpoint))
                yield return Result.Failure("The write endpoint address was not provided.");
        }

        public override IModelTypeDefinition Build()
        {
            return new ModelTypeDefinition(_modelType)
            {
                TableName = TableName,
                TimeToLive = _timeToLive,
                UseMemoryCache = _useMemoryCache,
                WriteBufferSize = _writeBufferSize,
                ReadBufferSize = _readBufferSize,
                DbInstance = _dbInstance,
                ReadEndpoint = _readEndpoint,
                WriteEndpoint = _writeEndpoint
            };
        }
    }
}