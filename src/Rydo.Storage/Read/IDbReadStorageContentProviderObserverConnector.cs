namespace Rydo.Storage.Read
{
    using Abstractions.Observers;
    using Abstractions.Utils;

    public interface IDbReadStorageContentProviderObserverConnector
    {
        IConnectHandle ConnectDbReadStorageContentProviderObserver(IDbReadStorageContentProviderObserver observer);
    }
}