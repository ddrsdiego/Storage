namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using Ardalis.ApiEndpoints;
    using Core.Models;
    using Microsoft.AspNetCore.Mvc;

    public sealed class PostCustomersPosition : EndpointBaseAsync
        .WithRequest<string>
        .WithActionResult
    {
        private readonly IStorageClient _storageClient;

        public PostCustomersPosition(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpPost("api/v1/customers/position/{accountNumber}")]
        public override async Task<ActionResult> HandleAsync(string accountNumber,
            CancellationToken cancellationToken = new())
        {
            const decimal minValue = 1.00m;
            const decimal maxValue = 99999.00m;
            const int decimalPlaces = 2;

            var rand = new Random();

            var balance = Math.Round((decimal) rand.NextDouble() * (maxValue - minValue) + minValue, decimalPlaces);
            var customerPosition = new CustomerPosition
            {
                AccountNumber = accountNumber,
                Balance = balance
            };

            await _storageClient.Write.Upsert(customerPosition.AccountNumber, customerPosition, cancellationToken);
            return Created("", customerPosition);
        }
    }
}