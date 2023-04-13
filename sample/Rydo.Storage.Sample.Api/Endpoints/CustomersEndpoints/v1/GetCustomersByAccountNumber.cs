namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using Ardalis.ApiEndpoints;
    using Core.Models;
    using Microsoft.AspNetCore.Mvc;

    public sealed class GetCustomersByAccountNumber : EndpointBaseAsync
        .WithRequest<string>
        .WithoutResult
    {
        private readonly IStorageClient _storageClient;

        public GetCustomersByAccountNumber(IStorageClient storageClient)
        {
            _storageClient = storageClient ?? throw new ArgumentNullException(nameof(storageClient));
        }

        [HttpGet("api/v1/customers/{accountNumber}")]
        public override async Task HandleAsync(string accountNumber, CancellationToken cancellationToken = new())
        {
            var response = await _storageClient.Reader.Read<Customer>(accountNumber, cancellationToken);
            await response.WriteToPipeAsync(Response, cancellationToken: cancellationToken);
        }
    }
}