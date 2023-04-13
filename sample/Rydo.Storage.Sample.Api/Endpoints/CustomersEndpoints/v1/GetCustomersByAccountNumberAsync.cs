namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using Ardalis.ApiEndpoints;
    using Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using Read;

    public sealed class GetCustomersByAccountNumberAsync: EndpointBaseAsync
        .WithRequest<string>
        .WithoutResult
    {
        private readonly IStorageClient _storageClient;

        public GetCustomersByAccountNumberAsync(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpGet("api/v1/customers/async/{accountNumber}")]
        public override Task HandleAsync(string accountNumber, CancellationToken cancellationToken = new())
        {
            var futureReadResponse = FutureReadResponse.GetInstance();

            _ = _storageClient.Reader.ReadAsync<Customer>(accountNumber, futureReadResponse, cancellationToken);
            return futureReadResponse.ReadTask.WriteToPipeAsync(Response, cancellationToken: cancellationToken);
        }
    }
}