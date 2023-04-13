namespace Rydo.Storage.Read.Observers
{
    using Rydo.Storage.Abstractions.Observers;
    using Abstractions.Utils;

    public interface IDbReadStorageContentProviderObserverConnector
    {
        IConnectHandle ConnectDbReadStorageContentProviderObserver(IDbReadStorageContentProviderObserver observer);
    }
}