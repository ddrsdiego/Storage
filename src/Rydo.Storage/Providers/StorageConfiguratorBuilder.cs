namespace Rydo.Storage.Providers
{
    using System;
    using System.Collections.Immutable;

    public abstract class StorageConfiguratorBuilder<TModelType> : IStorageConfiguratorBuilder<TModelType>
        where TModelType : IModelTypeDefinitionBuilder
    {
        private const int ReadBufferSizeDefault = 2_000;
        private const int WriteBufferSizeDefault = 2_000;

        protected StorageConfiguratorBuilder(string storageType)
        {
            StorageType = storageType;
            UseMemoryCache = false;
            GetReadBufferSize = ReadBufferSizeDefault;
            GetWriteBufferSize = WriteBufferSizeDefault;
            Entries = ImmutableDictionary<string, IModelTypeDefinition>.Empty;
        }

        public string StorageType { get; }

        public bool UseMemoryCache { get; private set; }

        public int GetWriteBufferSize { get; private set; }

        public int GetReadBufferSize { get; private set; }

        public ImmutableDictionary<string, IModelTypeDefinition> Entries { get; protected set; }

        public IStorageConfiguratorBuilder<TModelType> WithUseMemoryCache()
        {
            UseMemoryCache = true;
            return this;
        }

        public IStorageConfiguratorBuilder<TModelType> SetWriteBufferSize(int writeBufferSize)
        {
            GetWriteBufferSize = writeBufferSize;
            return this;
        }

        public IStorageConfiguratorBuilder<TModelType> SetReadBufferSize(int readBufferSize)
        {
            GetReadBufferSize = readBufferSize;
            return this;
        }

        public abstract void TryAddModelType<T>(Action<TModelType> definition = default);
    }
}