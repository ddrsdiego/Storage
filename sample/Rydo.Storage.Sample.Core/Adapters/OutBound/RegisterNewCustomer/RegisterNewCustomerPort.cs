namespace Rydo.Storage.Sample.Core.Adapters.OutBound.RegisterNewCustomer
{
    using System.Threading.Tasks;
    using Domain.CustomerAggregate;
    using Domain.CustomerAggregate.Ports.OutBound.RegisterNewCustomer;
    
    public class RegisterNewCustomerPort : IRegisterNewCustomerAdapter
    {
        public Task RegisterNewCustomer(Customer customer)
        {
            throw new System.NotImplementedException();
        }
    }
}