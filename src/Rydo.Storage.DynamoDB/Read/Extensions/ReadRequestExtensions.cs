namespace Rydo.Storage.DynamoDB.Read.Extensions
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Amazon.DynamoDBv2.Model;
    using Internals.Metrics;
    using Rydo.Storage.Read;

    internal static class ReadRequestExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static GetItemRequest ToGetItemRequest(this ReadRequest readRequest, string tableName)
        {
            var storageKey = readRequest.ToStorageItemKey();

            var request = new GetItemRequest
            {
                TableName = tableName,
                Key = new Dictionary<string, AttributeValue>
                {
                    {
                        DynamoDbAttributeNames.PartitionKey, new AttributeValue
                        {
                            S = storageKey.Value
                        }
                    }
                }
            };

            return request;
        }
        
        public static void PushMetricForKeyNotFound(this ReadRequest readRequest, string tableName)
        {
            var storageItemKey = readRequest.ToStorageItemKey();
            PrometheusMetrics.ReadRequestNotFound(storageItemKey.Value!, tableName);
        }
    }
}