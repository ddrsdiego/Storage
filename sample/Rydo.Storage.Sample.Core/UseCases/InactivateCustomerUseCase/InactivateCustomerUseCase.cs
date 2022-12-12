namespace Rydo.Storage.Sample.Core.UseCases.InactivateCustomerUseCase
{
    using System.Threading.Tasks;
    using Domain.CustomerAggregate.Ports.InBound.InactivateCustomerUseCase;
    using Domain.CustomerAggregate.Ports.OutBound.InactivateCustomer;

    public class InactivateCustomerUseCase : IInactivateCustomerUseCasePort
    {
        private readonly IInactivateCustomerAdapter _inactivateCustomerAdapter;
        
        public InactivateCustomerUseCase(IInactivateCustomerAdapter inactivateCustomerAdapter)
        {
            _inactivateCustomerAdapter = inactivateCustomerAdapter;
        }
        
        public Task<InactivateCustomerResponse> Inactivate(InactivateCustomerRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}