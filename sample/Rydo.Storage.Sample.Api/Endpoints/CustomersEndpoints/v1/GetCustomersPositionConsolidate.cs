namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using Ardalis.ApiEndpoints;
    using Microsoft.AspNetCore.Mvc;
    using Read;
    using Core.Models;

    public sealed class GetCustomersPositionConsolidate : EndpointBaseAsync
        .WithRequest<string>
        .WithoutResult
    {
        private readonly IStorageClient _storageClient;
        private readonly ILogger<GetCustomersPositionConsolidate> _logger;

        public GetCustomersPositionConsolidate(IStorageClient storageClient,
            ILogger<GetCustomersPositionConsolidate> logger)
        {
            _storageClient = storageClient;
            _logger = logger;
        }

        [HttpGet("api/v1/customers/position/consolidate/{accountNumber}")]
        public override Task HandleAsync(string accountNumber, CancellationToken cancellationToken = new())
        {
            var futureReadResponse = FutureReadResponse.GetInstance();

            _ = _storageClient.Reader.ReadAsync<CustomerPositionConsolidated>(accountNumber, futureReadResponse, cancellationToken);

            return futureReadResponse.ReadTask.WriteResponseAsync(Response, cancellationToken);
        }
    }
}