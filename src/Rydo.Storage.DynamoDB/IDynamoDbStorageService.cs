namespace Rydo.Storage.DynamoDB
{
    using Amazon.DynamoDBv2;
    using Providers;

    public interface IDynamoDbStorageService : IStorageService
    {
        IAmazonDynamoDB DynamoDb { get; }
    }
}