namespace Rydo.Storage.Sample.Api.Endpoints.CustomersEndpoints
{
    using Core.Models;

    internal static class CustomerModelHelper
    {
        public static List<Customer> GetCustomers(int amount)
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