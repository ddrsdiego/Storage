namespace Rydo.Storage.Sample.Core.Domain.CustomerAggregate.Ports.InBound.CreateCustomerUseCase
{
    using System.Threading.Tasks;

    public interface IUseCase<in TReq, TRsp>
        where TReq : IUseCaseRequest<TRsp>
        where TRsp : IUseCaseResponse
    {
        Task<TRsp> Execute(TReq request);
    }

    public interface IUseCaseRequest<TRsp>
        where TRsp : IUseCaseResponse
    {
        public TRsp Response { get; set; }
    }

    public interface IUseCaseResponse
    {
    }

    public interface ICreateCustomerUseCasePort : IUseCase<CreateCustomerRequest, CreateCustomerResponse>
    {
    }
}