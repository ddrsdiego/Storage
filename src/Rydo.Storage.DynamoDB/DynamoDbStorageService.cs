namespace Rydo.Storage.DynamoDB
{
    using Amazon.DynamoDBv2;
     
    internal sealed class DynamoDbStorageService : IDynamoDbStorageService
    {
        public DynamoDbStorageService(IAmazonDynamoDB dynamoDb)
        {
            DynamoDb = dynamoDb;
        }

        public IAmazonDynamoDB DynamoDb { get; }
    }
}