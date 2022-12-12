namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAll();

        Task<Customer> GetByAccountNumber(string accountNumber);

        Task Register(Customer newCustomer);
    }

    public sealed class CustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers;

        public CustomerRepository()
        {
            const int capacity = 10;

            var random = new Random();
            _customers = new List<Customer>(10);

            for (var i = 0; i < capacity; i++)
            {
                var age = random.Next(15, 50);
                var yearsToBeRemove = -1 * age;

                // _customers.Add(new Customer
                // {
                //     AccountNumber = i.ToString("000"),
                //     Name = $"CUSTOMER-NAME-{i}",
                //     Age = age,
                //     BirthDate = DateTime.Today.AddYears(yearsToBeRemove)
                // });
            }
        }

        public Task<List<Customer>> GetAll() => Task.FromResult(_customers);

        public Task<Customer> GetByAccountNumber(string accountNumber) =>
            Task.FromResult(_customers.FirstOrDefault(x =>
                x.AccountNumber.Value.Equals(accountNumber, StringComparison.InvariantCultureIgnoreCase)));

        public async Task Register(Customer newCustomer)
        {
            var customer = _customers.OrderByDescending(x => x.AccountNumber).First();
            var accountNumber = Convert.ToInt32(customer.AccountNumber) + 1;

            // newCustomer.AccountNumber = accountNumber.ToString("000");
            // _customers.Add(newCustomer);

            await Task.CompletedTask;
        }
    }
}