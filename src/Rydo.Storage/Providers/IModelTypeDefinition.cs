namespace Rydo.Storage.Providers
{
    using System;

    public interface IModelTypeDefinition
    {
        Type ModelType { get; }
        string ModeTypeName { get; }
        string TableName { get; }
        TimeSpan TimeToLive { get; }
        string WriteEndpoint { get; }
        string ReadEndpoint { get; }
        bool UseMemoryCache { get; }
        string DbInstance { get; }
        int WriteBufferSize { get; }
        int ReadBufferSize { get; }
        string AccessKey { get; }
        string SecretKey { get; }
    }
}