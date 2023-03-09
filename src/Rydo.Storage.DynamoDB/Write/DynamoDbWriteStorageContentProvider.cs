namespace Rydo.Storage.DynamoDB.Write
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Amazon.DynamoDBv2.Model;
    using Extensions;
    using Helpers;
    using Rydo.Storage.Extensions;
    using Rydo.Storage.Write;
    using WriteRequest = Storage.Write.WriteRequest;
    //
    // public interface IDynamoDbWriteStorageContentProvider
    // {
    //     Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default);
    // }

    internal sealed class DynamoDbWriteStorageContentProvider : IDbWriteStorageContentProvider
    {
        private readonly IModelTypeContextContainer _modelTypeContextContainer;
        private IDynamoDbStorageService _storageService;

        public DynamoDbWriteStorageContentProvider(IModelTypeContextContainer modelTypeContextContainer)
        {
            _modelTypeContextContainer = modelTypeContextContainer;
        }

        public async Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            if (!_modelTypeContextContainer.TryGetModel(writeBatchRequest.ModeTypeName!, out var modelTypeContext))
                throw new InvalidOperationException("topicDefinition.TopicName");

            _storageService = (IDynamoDbStorageService) modelTypeContext.StorageService;

            var tableName = modelTypeContext.Definition.TableName;

            var tasks = InitWriteTasks(tableName, writeBatchRequest, cancellationToken);
            foreach (var (writeRequest, task) in tasks)
            {
                try
                {
                    await task.Run().WaitCompletedSuccessfully();

                    var dynamoDbResponse = writeRequest.Operation switch
                    {
                        WriteRequestOperation.Upsert => await ((Future<PutItemResponse>) task).ToDynamoDbResponse(),
                        WriteRequestOperation.Remove => await ((Future<DeleteItemResponse>) task).ToDynamoDbResponse(),
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    _ = writeRequest.Response.TrySetResult(
                        dynamoDbResponse.StatusCode == HttpStatusCode.OK
                            ? WriteResponse.GetCreatedInstance(writeRequest)
                            : WriteResponse.GetErrorInstance(writeRequest, new Exception()));
                }
                catch (Exception e)
                {
                    _ = writeRequest.Response
                        .TrySetResult(WriteResponse.GetErrorInstance(writeRequest, e));
                }
            }
        }

        private IDictionary<WriteRequest, IFuture> InitWriteTasks(string tableName,
            IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            var futuresTasks = new Dictionary<WriteRequest, IFuture>(writeBatchRequest.Count);
            foreach (var writeRequest in writeBatchRequest)
            {
                try
                {
                    var storageItem = writeRequest.ToStorageItem();

                    switch (writeRequest.Operation)
                    {
                        case WriteRequestOperation.Upsert:
                        {
                            var putRequest = storageItem.ToPutItemRequest(tableName);

                            futuresTasks[writeRequest] = new Future<PutItemResponse>(
                                () => _storageService.DynamoDb.PutItemAsync(putRequest, cancellationToken),
                                cancellationToken);

                            break;
                        }
                        case WriteRequestOperation.Remove:
                        {
                            var deleteRequest = storageItem.ToDeleteRequest(tableName);

                            futuresTasks[writeRequest] = new Future<DeleteItemResponse>(
                                () => _storageService.DynamoDb.DeleteItemAsync(deleteRequest, cancellationToken),
                                cancellationToken);

                            break;
                        }

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return futuresTasks;
        }
    }

    internal class DynamoDbResponse
    {
        public DynamoDbResponse(HttpStatusCode statusCode, Exception exception = null)
        {
            Exception = exception;
            StatusCode = statusCode;
        }

        public readonly HttpStatusCode StatusCode;
        public readonly Exception Exception;
    }
}