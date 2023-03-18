namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using System.Diagnostics;
    using Ardalis.ApiEndpoints;
    using Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using Read;

    public sealed class GetCustomers : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult
    {
        private readonly ILogger<GetCustomers> _logger;
        private readonly IStorageClient _storageClient;

        public GetCustomers(ILogger<GetCustomers> logger, IStorageClient storageClient)
        {
            _logger = logger;
            _storageClient = storageClient;
        }

        [HttpGet("api/v1/customers/{amount:int}")]
        public override async Task<ActionResult> HandleAsync(int amount, CancellationToken cancellationToken = new CancellationToken())
        {
            var customers = CustomerModelHelper.GetCustomers(amount);

            var sw = Stopwatch.StartNew();
            var tasks = new List<ValueTask<ReadResponse>>(customers.Count);
            foreach (var customer in customers)
            {
                var task = _storageClient.Reader.Read<Customer>(customer.AccountNumber, cancellationToken);
                tasks.Add(task);
            }

            foreach (var task in tasks)
            {
                var response = await task;
                // _logger.LogInformation("ElapsedMilliseconds: {ResponseElapsedMilliseconds}", response.ElapsedMilliseconds);
            }

            sw.Stop();
            _logger.LogInformation("ElapsedMilliseconds: {SwElapsedMilliseconds}", sw.ElapsedMilliseconds);

            return Ok(customers.LastOrDefault());
        }
    }
}