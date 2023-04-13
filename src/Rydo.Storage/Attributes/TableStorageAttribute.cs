namespace Rydo.Storage.Attributes
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public sealed class TableStorageAttribute : Attribute
    {
        internal const int TableNamePosition = 0;
        internal const string FullNameTopicProducerAttribute = "Rydo.Storage.Attributes.TableStorageAttribute";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TableStorageAttribute(string tableName)
        {
            TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        internal string TableName { get; }
    }

    internal static class ModelExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryExtractTableName(this object model, out string tableName)
        {
            tableName = string.Empty;

            if (model == null)
                return false;

            if (!model.GetType().CustomAttributes.Any())
            {
                tableName = string.Empty;
                return false;
            }

            var attr = model.GetType()
                .CustomAttributes
                .SingleOrDefault(x =>
                    x.AttributeType.FullName is TableStorageAttribute.FullNameTopicProducerAttribute);

            if (attr == null)
            {
                tableName = string.Empty;
                return false;
            }

            tableName = attr.ConstructorArguments[TableStorageAttribute.TableNamePosition].Value.ToString();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool TryExtractTableName<T>(out string tableName)
        {
            tableName = string.Empty;

            if (!typeof(T).CustomAttributes.Any())
            {
                tableName = string.Empty;
                return false;
            }

            var attr = typeof(T)
                .CustomAttributes
                .SingleOrDefault(x =>
                    x.AttributeType.FullName is TableStorageAttribute.FullNameTopicProducerAttribute);

            if (attr == null)
            {
                tableName = string.Empty;
                return false;
            }

            tableName = attr.ConstructorArguments[TableStorageAttribute.TableNamePosition].Value.ToString();
            return true;
        }
    }
}