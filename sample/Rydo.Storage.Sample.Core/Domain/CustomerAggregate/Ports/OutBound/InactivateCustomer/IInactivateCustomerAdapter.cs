namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate.Ports.OutBound.InactivateCustomer
{
    using System.Threading.Tasks;
    using Models;

    public interface IInactivateCustomerAdapter
    {
        Task Inactivate(Customer customer);
    }
}