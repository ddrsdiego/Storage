namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints.v1
{
    using Ardalis.ApiEndpoints;
    using Microsoft.AspNetCore.Mvc;
    using Write;

    public sealed class PostCustomers : EndpointBaseAsync
        .WithRequest<int>
        .WithActionResult
    {
        private readonly IStorageClient _storageClient;

        public PostCustomers(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpPost("api/v1/customers/{amount:int}")]
        public override async Task<ActionResult> HandleAsync(int amount, CancellationToken cancellationToken = new())
        {
            var customers = CustomerModelHelper.GetCustomers(amount);

            var tasks = new List<ValueTask<WriteResponse>>(customers.Count);
            foreach (var customer in customers)
            {
                tasks.Add(_storageClient.Write.Upsert(customer.AccountNumber, customer, cancellationToken));
            }

            foreach (var task in tasks)
            {
                var response = await task;
            }

            return Created("", customers.LastOrDefault());
        }
    }
}