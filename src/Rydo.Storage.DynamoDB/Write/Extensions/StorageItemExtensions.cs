namespace Rydo.Storage.DynamoDB.Write.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Amazon.DynamoDBv2.Model;

    internal static class StorageItemExtensions
    {
        public static DeleteItemRequest ToDeleteRequest(this StorageItem storageItem, string tableName)
        {
            if (tableName == null || string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var request = new DeleteItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        DynamoDbAttributeNames.PartitionKey, new AttributeValue
                        {
                            S = storageItem.Key.Value
                        }
                    }
                }
            };

            return request;
        }

        public static PutItemRequest ToPutItemRequest(this StorageItem storageItem, string tableName)
        {
            if (tableName == null || string.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            var request = storageItem.Key.IsComposed
                ? CreateRequestWithSortKey(storageItem, tableName)
                : CreateRequestDefault(storageItem, tableName);

            return CreateRequestDefault(storageItem, tableName);
        }

        private static PutItemRequest CreateRequestDefault(StorageItem storageItem, string tableName)
        {
            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                        DynamoDbAttributeNames.PartitionKey, new AttributeValue
                        {
                            S = storageItem.Key.Value
                        }
                    },
                    {
                        DynamoDbAttributeNames.CreatedAt, new AttributeValue
                        {
                            S = DateTime.Now.ToString("o")
                        }
                    },
                    {
                        DynamoDbAttributeNames.IsDisable, new AttributeValue
                        {
                            BOOL = false
                        }
                    },
                    {
                        DynamoDbAttributeNames.Payload, new AttributeValue
                        {
                            B = new MemoryStream(storageItem.Payload.ToArray())
                        }
                    }
                }
            };
            return request;
        }

        private static PutItemRequest CreateRequestWithSortKey(StorageItem storageItem, string tableName)
        {
            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {
                        DynamoDbAttributeNames.PartitionKey, new AttributeValue
                        {
                            S = storageItem.Key.Key
                        }
                    },
                    {
                        DynamoDbAttributeNames.SortKey, new AttributeValue
                        {
                            S = storageItem.Key.SortKey
                        }
                    },
                    {
                        DynamoDbAttributeNames.CreatedAt, new AttributeValue
                        {
                            S = DateTime.Now.ToString("o")
                        }
                    },
                    {
                        DynamoDbAttributeNames.Payload, new AttributeValue
                        {
                            B = new MemoryStream(storageItem.Payload.ToArray())
                        }
                    }
                }
            };
            return request;
        }
    }
}