namespace Rydo.Storage.Providers
{
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;

    public interface IModelTypeDefinitionBuilder
    {   
        IEnumerable<Result> Validate();
        
        IModelTypeDefinition Build();
    }

    public abstract class ModelTypeDefinitionBuilder : IModelTypeDefinitionBuilder
    {
        protected ModelTypeDefinitionBuilder(string? tableName)
        {
            TableName = tableName;
        }

        protected string? TableName { get; }
        
        public abstract IEnumerable<Result> Validate();

        public abstract IModelTypeDefinition Build();
    }
}