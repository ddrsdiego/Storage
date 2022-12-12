namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate.Ports.InBound.CreateCustomerUseCase
{
    using System;

    public sealed class CreateCustomerRequest : IUseCaseRequest<CreateCustomerResponse>
    {
        public CreateCustomerRequest(string firstName, string lastName, string email, DateTime birthDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            BirthDate = birthDate;
            Response = new CreateCustomerResponse();
        }
        
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime BirthDate { get; }
        public CreateCustomerResponse Response { get; set; }
    }
}