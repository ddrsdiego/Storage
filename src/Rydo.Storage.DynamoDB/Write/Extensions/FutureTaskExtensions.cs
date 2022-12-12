namespace Rydo.Storage.DynamoDB.Write.Extensions
{
    using System.Threading.Tasks;
    using Amazon.DynamoDBv2.Model;
    using Helpers;
    using Storage.Extensions;

    internal static class FutureTaskExtensions
    {
        public static async Task<DynamoDbResponse> ToDynamoDbResponse(this Future<PutItemResponse> task)
        {
            var response = await task.Completed.WaitCompletedSuccessfully();
            return new DynamoDbResponse(response.HttpStatusCode);
        }
        
        public static async Task<DynamoDbResponse> ToDynamoDbResponse(this Future<DeleteItemResponse> task)
        {
            var response = await task.Completed.WaitCompletedSuccessfully();
            return new DynamoDbResponse(response.HttpStatusCode);
        }
    }
}