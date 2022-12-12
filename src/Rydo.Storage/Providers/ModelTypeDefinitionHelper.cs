namespace Rydo.Storage.Providers
{
    using System;

    internal static class ModelTypeDefinitionHelper
    {
        public static string SanitizeModeTypeName(Type modelType)
        {
            const string suffixModelType = "type-model";
            const string oldValue = ".";
            const string newValue = "-";

            var modelName = modelType.FullName?.Replace(oldValue, newValue).ToLowerInvariant();
            
            return $"{modelName}-{suffixModelType}";
        }
    }
}