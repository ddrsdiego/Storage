namespace Rydo.Storage.Write
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDbWriteStorageContentProvider
    {
        Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default);
    }
}