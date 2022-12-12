namespace Rydo.Storage.Providers
{
    using System;
    using System.Collections.Immutable;

    public interface IStorageConfiguratorBuilder
    {
        string StorageType { get; }

        bool UseMemoryCache { get; }

        int GetWriteBufferSize { get; }

        int GetReadBufferSize { get; }
    }

    public interface IStorageConfiguratorBuilder<out TModelType> : IStorageConfiguratorBuilder
        where TModelType : IModelTypeDefinitionBuilder
    {
        ImmutableDictionary<string, IModelTypeDefinition> Entries { get; }

        IStorageConfiguratorBuilder<TModelType> WithUseMemoryCache();

        IStorageConfiguratorBuilder<TModelType> SetWriteBufferSize(int writeBufferSize);

        IStorageConfiguratorBuilder<TModelType> SetReadBufferSize(int readBufferSize);

        void TryAddModelType<T>(Action<TModelType>? definition = default);
    }
}