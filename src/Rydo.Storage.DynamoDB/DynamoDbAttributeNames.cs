namespace Rydo.Storage.DynamoDB
{
    internal static class DynamoDbAttributeNames
    {
        public const string PartitionKey = "Key";
        public const string SortKey = nameof(SortKey);
        public const string Payload = nameof(Payload);
        public const string CreatedAt = nameof(CreatedAt);
        public const string IsDisable = nameof(IsDisable);
        public const string LastUpdated = nameof(LastUpdated);
    }
}