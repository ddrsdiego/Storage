namespace Rydo.Storage.Sample.Api.Controllers
{
    using System.Diagnostics;
    using System.Text;
    using Core.Domain.CustomerAggregate;
    using Core.Models;
    using Microsoft.AspNetCore.Mvc;
    using Read;
    using Write;
    using Customer = Core.Models.Customer;

    [ApiController]
    [Route("customers")]
    public class CustomerController : Controller
    {
        private readonly IStorageClient _storageClient;
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ILogger<CustomerController> logger, ICustomerRepository customerRepository,
            IStorageClient storageClient)
        {
            _logger = logger;
            _customerRepository = customerRepository;
            _storageClient = storageClient;
        }

        [HttpGet("{accountNumber}")]
        public async ValueTask GetByAccountNumber(string accountNumber)
        {
            var response = await _storageClient.Reader.Read<Customer>(accountNumber);
            response.Value.WriteToPipe(Response.BodyWriter);
        }

        [HttpGet("async/{accountNumber}")]
        public ValueTask GetFutureResponse(string accountNumber)
        {
            var futureReadResponse = FutureReadResponse.GetInstance();

            _ = _storageClient.Reader.ReadAsync<Customer>(accountNumber, futureReadResponse);

            return futureReadResponse.ReadTask.WriteResponseAsync(Response);
        }

        [HttpGet("/amount/{amount:int}")]
        public async Task<IActionResult> GetByAmount(int amount)
        {
            var customers = GetCustomers(amount);

            var sw = Stopwatch.StartNew();
            var tasks = new List<ValueTask<ReadResponse>>(customers.Count);
            foreach (var customer in customers)
            {
                var task = _storageClient.Reader.Read<Customer>(customer.AccountNumber);
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

        [HttpPost("{amount:int}")]
        public async Task<IActionResult> Register(int amount)
        {
            var customers = GetCustomers(amount);

            var tasks = new List<ValueTask<WriteResponse>>(customers.Count);
            foreach (var customer in customers)
            {
                tasks.Add(_storageClient.Write.Upsert(customer.AccountNumber, customer));
            }

            foreach (var task in tasks)
            {
                var response = await task;
            }

            return Created("", customers.LastOrDefault());
        }

        [HttpPost("/position/{accountNumber}")]
        public async Task<IActionResult> UpdateBalance(string accountNumber)
        {
            var random = new Random();
            var customerPosition = new CustomerPosition
            {
                AccountNumber = accountNumber,
                Balance = (decimal) random.NextDouble()
            };

            await _storageClient.Write.Upsert<CustomerPosition>(customerPosition.AccountNumber, customerPosition);
            return Created("", customerPosition);
        }

        [HttpPost("/position/consolidate/{accountNumber}")]
        public async Task<IActionResult> UpdatePosition(string accountNumber)
        {
            var futureResponseCustomer = FutureReadResponse.GetInstance();
            var futureResponsePosition = FutureReadResponse.GetInstance();

            _ = _storageClient.Reader.ReadAsync<Customer>(accountNumber, futureResponseCustomer);
            _ = _storageClient.Reader.ReadAsync<CustomerPosition>(accountNumber, futureResponsePosition);

            var customer = await futureResponseCustomer.ReadTask;
            var customerPosition = await futureResponsePosition.ReadTask;

            var customerPositionConsolidated = new CustomerPositionConsolidated
            {
                Customer = customer.GetRaw<Customer>(),
                Position = customerPosition.GetRaw<CustomerPosition>()
            };

            var response = await _storageClient.Write.Upsert(accountNumber, customerPositionConsolidated);
            if (response.Status == WriteResponseStatus.Created)
                return Created("", response.Request.GetRaw<CustomerPositionConsolidated>());

            return BadRequest();
        }

        [HttpGet("/position/consolidate/{accountNumber}")]
        public ValueTask GetPosition(string accountNumber)
        {
            var futureReadResponse = FutureReadResponse.GetInstance();

            _ = _storageClient.Reader.ReadAsync<CustomerPositionConsolidated>(accountNumber, futureReadResponse);

            return futureReadResponse.ReadTask.WriteResponseAsync(Response);
        }

        private static List<Customer> GetCustomers(int amount)
        {
            var customers = new List<Customer>(amount);
            var random = new Random();

            for (var counter = 1; counter <= amount; counter++)
            {
                var age = random.Next(15, 50);
                var yearsToBeRemove = -1 * age;

                customers.Add(new Customer
                {
                    AccountNumber = counter.ToString("000000"),
                    Name = $"CUSTOMER-NAME-{counter}",
                    Age = age,
                    BirthDate = DateTime.Today.AddYears(yearsToBeRemove)
                });
            }

            return customers;
        }
    }
}