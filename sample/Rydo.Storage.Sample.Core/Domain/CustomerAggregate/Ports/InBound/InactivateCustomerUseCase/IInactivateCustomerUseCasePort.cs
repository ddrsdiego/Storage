namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate.Ports.InBound.InactivateCustomerUseCase
{
    using System.Threading.Tasks;

    public interface IInactivateCustomerUseCasePort
    {
        Task<InactivateCustomerResponse> Inactivate(InactivateCustomerRequest request);
    }
}