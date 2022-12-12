namespace Rydo.Storage.MongoDb
{
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Providers;

    public class MongoModelTypeDefinitionBuilder : IModelTypeDefinitionBuilder
    {
        private string _connectionString;

        public IEnumerable<Result> Validate()
        {
            return null;
        }

        public IModelTypeDefinition Build()
        {
            throw new System.NotImplementedException();
        }

        public MongoModelTypeDefinitionBuilder ConnectionString(string connectionString)
        {
            _connectionString = connectionString;
            return this;
        }
    }
}