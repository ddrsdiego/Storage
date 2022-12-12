namespace Rydo.Storage.Sample.Core.Adapters.OutBound.InactivateCustomer
{
    using System.Threading.Tasks;
    using Domain.CustomerAggregate.Ports.OutBound.InactivateCustomer;
    using Models;

    public class InactivateCustomerPort : IInactivateCustomerAdapter
    {
        public Task Inactivate(Customer customer)
        {
            throw new System.NotImplementedException();
        }
    }
}