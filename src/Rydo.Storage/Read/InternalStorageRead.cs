namespace Rydo.Storage.Read
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Providers;

    public interface IStorageRead
    {
        Task Execute(ReadBatchRequest batch, CancellationToken cancellationToken = default);
    }

    internal sealed class InternalStorageRead : IStorageRead
    {
        private readonly IStorageContentProvider _storageContentProvider;

        public InternalStorageRead(IStorageContentProvider storageContentProvider)
        {
            _storageContentProvider =
                storageContentProvider ?? throw new ArgumentNullException(nameof(storageContentProvider));
        }

        public Task Execute(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            return _storageContentProvider.Read(batch, cancellationToken);
        }
    }
}