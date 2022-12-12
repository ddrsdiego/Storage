namespace Rydo.Storage.DynamoDB.Read.Extensions
{
    using System.Net;
    using System.Runtime.CompilerServices;
    using Amazon.DynamoDBv2.DocumentModel;
    using Amazon.DynamoDBv2.Model;
    using Storage.Read;

    internal static class GetItemResponseExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadResponse CreateReadResponse(this GetItemResponse response, ReadRequest readRequest)
        {
            ReadResponse readResponse;

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                var itemAsDocument = Document.FromAttributeMap(response.Item);

                readResponse = itemAsDocument.Count == 0
                    ? ReadResponse.GetResponseNotFound(readRequest)
                    : ReadResponse.GetResponseOk(readRequest, (byte[]) itemAsDocument[DynamoDbAttributeNames.Payload]);
            }
            else
                readResponse = ReadResponse.GetResponseNotFound(readRequest);

            return readResponse;
        }
    }
}