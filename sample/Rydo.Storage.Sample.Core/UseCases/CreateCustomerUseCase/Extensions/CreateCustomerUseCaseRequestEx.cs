namespace Rydo.Storage.Sample.Core.UseCases.CreateCustomerUseCase.Extensions
{
    using Domain.CustomerAggregate;
    using Domain.CustomerAggregate.Ports.InBound.CreateCustomerUseCase;


    internal static class CreateCustomerUseCaseRequestEx
    {
        public static Customer ToCustomer(this CreateCustomerRequest request, string accountNumber)
        {
            var fullName = new CustomerName(request.FirstName, request.LastName);
            var newAccountNumber = new AccountNumber(accountNumber, 0);
            var newCustomer = new Customer(newAccountNumber, fullName, new Email(request.Email), request.BirthDate);

            return newCustomer;
        }
    }
}