namespace Rydo.Storage
{
    using Read;
    using Write;

    public interface IStorageClient
    {
        IStorageClientReader Reader { get; }
        IStorageClientWrite Write { get; }
    }

    internal sealed class StorageClient : IStorageClient
    {
        public StorageClient(IStorageClientReader reader, IStorageClientWrite write)
        {
            Write = write;
            Reader = reader;
        }

        public IStorageClientReader Reader { get; }

        public IStorageClientWrite Write { get; }
    }
}