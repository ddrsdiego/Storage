namespace Rydo.Storage.Write
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IStorageWrite
    {
        Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default);
    }
}