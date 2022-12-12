// namespace Rydo.Storage.Providers
// {
//     using System;
//     using System.Collections.Generic;
//     using CSharpFunctionalExtensions;
//
//     public sealed class ModelTypeDefinitionBuilder : IModelTypeDefinitionBuilder
//     {
//         private const string DbInstanceDefault = "-1";
//
//         private readonly Type _modelType;
//         private TimeSpan _timeToLive;
//         private string? _writeEndpoint;
//         private string? _readEndpoint;
//         
//         private string? _accessKey;
//         private string? _secretKey;
//         
//         private string? _dbInstance;
//         private bool _useMemoryCache;
//         private int _writeBufferSize;
//         private int _readBufferSize;
//
//         public ModelTypeDefinitionBuilder(Type modelType)
//         {
//             _readEndpoint = string.Empty;
//             _writeEndpoint = string.Empty;
//             _dbInstance = DbInstanceDefault;
//             _modelType = modelType;
//             _writeBufferSize = 2_000;
//             _readBufferSize = 2_000;
//         }
//
//         public IModelTypeDefinitionBuilder TimeToLive(TimeSpan timeToLive)
//         {
//             _timeToLive = timeToLive;
//             return this;
//         }
//
//         public IModelTypeDefinitionBuilder WriteEndpoint(string? writeEndpoint)
//         {
//             if (string.IsNullOrEmpty(_writeEndpoint))
//                 _writeEndpoint = writeEndpoint;
//             return this;
//         }
//
//         public IModelTypeDefinitionBuilder ReadEndpoint(string? readEndpoint)
//         {
//             if (string.IsNullOrEmpty(_readEndpoint))
//                 _readEndpoint = readEndpoint;
//             return this;
//         }
//
//         public ModelTypeDefinitionBuilder DbInstance(string dbInstance)
//         {
//             _dbInstance = dbInstance;
//             return this;
//         }
//
//         public ModelTypeDefinitionBuilder SetAccessKey(string accessKey)
//         {
//             _accessKey = accessKey;
//             return this;
//         }
//
//         public ModelTypeDefinitionBuilder SetSecretKey(string secretKey)
//         {
//             _secretKey = secretKey;
//             return this;
//         }
//         
//         public IModelTypeDefinitionBuilder UseMemoryCache()
//         {
//             UseMemoryCache(true);
//             return this;
//         }
//
//         private void UseMemoryCache(bool useMemoryCache) => _useMemoryCache = useMemoryCache;
//
//         public IModelTypeDefinitionBuilder WriteBufferSize(int bufferSize)
//         {
//             _writeBufferSize = bufferSize;
//             return this;
//         }
//
//         public IModelTypeDefinitionBuilder ReadBufferSize(int bufferSize)
//         {
//             _readBufferSize = bufferSize;
//             return this;
//         }
//
//         public IEnumerable<Result> Validate()
//         {
//             throw new NotImplementedException();
//         }
//
//         public IModelTypeDefinition Build()
//         {
//             return new ModelTypeDefinition(_modelType)
//             {
//                 ReadEndpoint = _readEndpoint,
//                 WriteEndpoint = _writeEndpoint,
//                 TimeToLive = _timeToLive,
//                 UseMemoryCache = _useMemoryCache,
//                 DbInstance = _dbInstance,
//                 WriteBufferSize = _writeBufferSize,
//                 ReadBufferSize = _readBufferSize,
//                 AccessKey = _accessKey,
//                 SecretKey = _secretKey
//             };
//         }
//     }
// }