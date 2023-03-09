namespace Rydo.Storage.Postgres
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Providers;

    public class PostgresModelTypeDefinitionBuilder : ModelTypeDefinitionBuilder
    {
        public PostgresModelTypeDefinitionBuilder(Type type, string tableName) 
            : base(tableName)
        {
        }

        public override IEnumerable<Result> Validate()
        {
            throw new System.NotImplementedException();
        }

        public override IModelTypeDefinition Build()
        {
            throw new System.NotImplementedException();
        }
    }
}