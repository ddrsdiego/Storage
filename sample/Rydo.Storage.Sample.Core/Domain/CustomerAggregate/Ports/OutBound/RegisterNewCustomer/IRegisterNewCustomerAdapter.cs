namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate.Ports.OutBound.RegisterNewCustomer
{
    using System.Threading.Tasks;

    public interface IRegisterNewCustomerAdapter
    {
        Task RegisterNewCustomer(Customer newCustomer);
    }
}