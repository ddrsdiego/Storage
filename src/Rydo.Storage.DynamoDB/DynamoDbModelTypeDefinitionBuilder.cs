namespace Rydo.Storage.DynamoDB
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Providers;

    public sealed class DynamoDbModelTypeDefinitionBuilder : ModelTypeDefinitionBuilder
    {
        private string _accessKey;
        private string _secretKey;
        private readonly Type _modelType;
        private int _writeBufferSize;
        private int _readBufferSize;
        private TimeSpan _timeToLive;
        private bool _useMemoryCache;

        public DynamoDbModelTypeDefinitionBuilder(Type type, string tableName)
            : base(tableName)
        {
            _modelType = type;
            _accessKey = string.Empty;
            _secretKey = string.Empty;
        }

        public DynamoDbModelTypeDefinitionBuilder WriteBufferSize(int writeBufferSize)
        {
            _writeBufferSize = writeBufferSize;
            return this;
        }

        public DynamoDbModelTypeDefinitionBuilder ReadBufferSize(int readBufferSize)
        {
            _readBufferSize = readBufferSize;
            return this;
        }

        public DynamoDbModelTypeDefinitionBuilder TimeToLive(TimeSpan timeToLive)
        {
            _timeToLive = timeToLive;
            return this;
        }

        public DynamoDbModelTypeDefinitionBuilder SetAccessKey(string accessKey)
        {
            _accessKey = accessKey;
            return this;
        }

        public DynamoDbModelTypeDefinitionBuilder SetSecretKey(string secretKey)
        {
            _secretKey = secretKey;
            return this;
        }

        public DynamoDbModelTypeDefinitionBuilder UseMemoryCache()
        {
            UseMemoryCache(true);
            return this;
        }

        internal DynamoDbModelTypeDefinitionBuilder UseMemoryCache(bool useMemoryCache)
        {
            _useMemoryCache = useMemoryCache;
            return this;
        }

        public override IEnumerable<Result> Validate()
        {
            if (string.IsNullOrEmpty(_accessKey))
                yield return Result.Failure("The AccessKey was not provided.");

            if (string.IsNullOrEmpty(_secretKey))
                yield return Result.Failure("The SecretKey was not provided.");
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
                AccessKey = _accessKey,
                SecretKey = _secretKey
            };
        }
    }
}