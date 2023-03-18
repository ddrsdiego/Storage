namespace Rydo.Storage.Postgres
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Providers;

    public class PostgresModelTypeDefinitionBuilder : ModelTypeDefinitionBuilder
    {
        private readonly Type _modelType;
        private string _writeEndpoint;
        private string _readEndpoint;
        
        public PostgresModelTypeDefinitionBuilder(Type type, string tableName) 
            : base(tableName)
        {
            _modelType = type;
            _writeEndpoint = string.Empty;
            _readEndpoint = string.Empty;
        }

        public PostgresModelTypeDefinitionBuilder ReadEndpoint(string readEndpoint)
        {
            _readEndpoint = readEndpoint;
            return this;
        }

        public PostgresModelTypeDefinitionBuilder WriteEndpoint(string writeEndpoint)
        {
            _writeEndpoint = writeEndpoint;
            return this;
        }

        
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
                ReadEndpoint = _readEndpoint,
                WriteEndpoint = _writeEndpoint
            };
        }
    }
}