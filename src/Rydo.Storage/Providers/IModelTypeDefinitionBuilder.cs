namespace Rydo.Storage.Providers
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Memory;

    public interface IModelTypeDefinitionBuilder
    {   
        IEnumerable<Result> Validate();
        
        IModelTypeDefinition Build();
    }

    public abstract class ModelTypeDefinitionBuilder : IModelTypeDefinitionBuilder
    {
        protected ModelTypeDefinitionBuilder(string tableName)
        {
            TableName = tableName;
        }

        protected string TableName { get; }
        
        protected bool MemoryCache { get; private set; }

        public ModelTypeDefinitionBuilder UseMemoryCache()
        {
            UseMemoryCache(true);
            return this;
        }
        
        public ModelTypeDefinitionBuilder UseMemoryCache(Action<StorageMemoryCacheEntryOptions> options)
        {
            var storageMemoryCacheEntryOptions = new StorageMemoryCacheEntryOptions();
            options(storageMemoryCacheEntryOptions);
            
            UseMemoryCache(true);
            return this;
        }

        private ModelTypeDefinitionBuilder UseMemoryCache(bool useMemoryCache)
        {
            MemoryCache = useMemoryCache;
            return this;
        }
        
        public abstract IEnumerable<Result> Validate();

        public abstract IModelTypeDefinition Build();
    }
}