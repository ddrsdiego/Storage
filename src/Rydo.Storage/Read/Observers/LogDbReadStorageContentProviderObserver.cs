namespace Rydo.Storage.Read.Observers
{
    using System.Threading.Tasks;
    using Abstractions.Observers;
    using Microsoft.Extensions.Logging;

    internal sealed class LogDbReadStorageContentProviderObserver :
        IDbReadStorageContentProviderObserver
    {
        private readonly ILogger<LogDbReadStorageContentProviderObserver> _logger;

        public LogDbReadStorageContentProviderObserver(ILoggerFactory logger) =>
            _logger = logger.CreateLogger<LogDbReadStorageContentProviderObserver>();

        public Task PreExecuteRead(ReadBatchRequest batch)
        {
            _logger.LogInformation("[{BatchId}] - Starting query for {BatchCount} item(s) on {TableName} table",
                batch.BatchId,
                batch.Count,
                batch.TableName);

            return Task.CompletedTask;
        }

        public Task PostExecuteRead(ReadBatchRequest batch)
        {
            batch.StopBatchReadRequestWatch();
            
            _logger.LogInformation(
                "[{BatchId}] - Finishing query for {BatchCount} item(s) on {TableName} table, Elapsed Time: {ElapsedMilliseconds}",
                batch.BatchId,
                batch.Count,
                batch.TableName,
                batch.ReadBatchRequestElapsedMilliseconds);

            return Task.CompletedTask;
        }
    }
}