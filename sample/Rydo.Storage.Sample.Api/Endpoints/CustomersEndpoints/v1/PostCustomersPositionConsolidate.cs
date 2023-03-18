namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using Ardalis.ApiEndpoints;
    using Microsoft.AspNetCore.Mvc;
    using Rydo.Storage.Read;
    using Rydo.Storage.Sample.Core.Models;
    using Rydo.Storage.Serialization;
    using Rydo.Storage.Write;

    public sealed class PostCustomersPositionConsolidate : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult
    {
        private readonly IStorageClient _storageClient;
        private readonly IRydoStorageCacheSerializer _serializer;
        private readonly ILogger<PostCustomersPositionConsolidate> _logger;
        
        public PostCustomersPositionConsolidate(IStorageClient storageClient,
            ILogger<PostCustomersPositionConsolidate> logger, IRydoStorageCacheSerializer serializer)
        {
            _storageClient = storageClient;
            _logger = logger;
            _serializer = serializer;
        }

        [HttpPost("api/v1/customers/position/consolidate/{accountNumber}")]
        public override async Task<ActionResult> HandleAsync(string accountNumber, CancellationToken cancellationToken = new())
        {
            var futureResponseCustomer = FutureReadResponse.GetInstance();
            var futureResponsePosition = FutureReadResponse.GetInstance();

            _ = _storageClient.Reader.ReadAsync<Customer>(accountNumber, futureResponseCustomer, cancellationToken);
            _ = _storageClient.Reader.ReadAsync<CustomerPosition>(accountNumber, futureResponsePosition, cancellationToken);

            var customer = await futureResponseCustomer.ReadTask;
            var customerPosition = await futureResponsePosition.ReadTask;

            var customerPositionConsolidated = new CustomerPositionConsolidated
            {
                Customer = await _serializer.DeserializeAsync<Customer>(customer.Value),
                Position = await _serializer.DeserializeAsync<CustomerPosition>(customerPosition.Value)
            };

            var response = await _storageClient.Write.Upsert(accountNumber, customerPositionConsolidated, cancellationToken);
            if (response.Status == WriteResponseStatus.Created)
                return Created("", response.Request.GetRaw<CustomerPositionConsolidated>());

            return BadRequest();
        }
    }
}