namespace Rydo.Storage.Sample.Core.UseCases.CreateCustomerUseCase
{
    using System.Threading.Tasks;
    using Domain.CustomerAggregate;
    using Domain.CustomerAggregate.Ports.InBound.CreateCustomerUseCase;
    using Domain.CustomerAggregate.Ports.OutBound.RegisterNewCustomer;
    using Extensions;

    public sealed class CreateCustomerUseCase : ICreateCustomerUseCasePort
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRegisterNewCustomerAdapter _registerNewCustomerAdapter;

        public CreateCustomerUseCase(ICustomerRepository customerRepository,
            IRegisterNewCustomerAdapter registerNewCustomerAdapter)
        {
            _customerRepository = customerRepository;
            _registerNewCustomerAdapter = registerNewCustomerAdapter;
        }

        public async Task<CreateCustomerResponse> Execute(CreateCustomerRequest request)
        {
            var rsp = request.Response;
            
            //TODO - Adicionar validações
            //1 - Validar de o usuário é maior que 18 anos.
            var age = new CustomerAge(request.BirthDate);
            if (age.IsNotMajor)
                return new CreateCustomerResponse();

            //2 - Validar se o email esta correto.
            var email = new Email(request.Email);
            if (email.IsNotValid)
                return new CreateCustomerResponse();

            //3 - Validar se o email já esta cadastrado na base de dados.

            var newCustomer = request.ToCustomer("5090016");

            await _customerRepository.Register(newCustomer);
            await _registerNewCustomerAdapter.RegisterNewCustomer(newCustomer);
            return new CreateCustomerResponse();
        }
    }
}