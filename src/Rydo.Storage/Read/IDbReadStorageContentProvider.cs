namespace Rydo.Storage.Read
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDbReadStorageContentProvider
    {
        Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default);
    }
}