namespace Rydo.Storage.Read.Observers
{
    using System.Threading.Tasks;
    using Abstractions.Observers;
    using Internals.Metrics;

    internal sealed class MetricDbReadStorageContentProviderObserver :
        IDbReadStorageContentProviderObserver
    {
        public Task PreExecuteRead(ReadBatchRequest batch) => Task.CompletedTask;

        public Task PostExecuteRead(ReadBatchRequest batch)
        {
            const int timeLimitDefault = 100;

            PrometheusMetrics.ReadRequestElapsedTimeExceeded(batch.TableName, timeLimitDefault);
            return Task.CompletedTask;
        }
    }
}