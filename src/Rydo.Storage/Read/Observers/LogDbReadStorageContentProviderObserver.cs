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
            _logger.LogDebug("[{BatchId}] - Starting query for {BatchCount} item(s) on {TableName} table",
                batch.BatchId,
                batch.Count,
                batch.TableName);

            return Task.CompletedTask;
        }

        public Task PostExecuteRead(ReadBatchRequest batch)
        {
            batch.StopBatchReadRequestWatch();

            var readRequestAudit = new ReadBatchRequestAudit(batch);

            _logger.LogInformation("[{LogType}] - {@ReadBatchRequestAudit}",
                readRequestAudit.LogType,
                readRequestAudit);

            return Task.CompletedTask;
        }
    }

    internal abstract class RequestAudit
    {
        protected RequestAudit(string logType) => LogType = logType;

        public string LogType { get; }
    }

    internal sealed class ReadBatchRequestAudit :
        RequestAudit
    {
        private readonly ReadBatchRequest _request;

        public ReadBatchRequestAudit(ReadBatchRequest request) :
            base("finish-read-request-audit") => _request = request;

        public string BatchId => _request.BatchId;
        public string TableName => _request.TableName;
        public int Lenght => _request.Count;
        public long ElapsedMilliseconds => _request.ReadBatchRequestElapsedMilliseconds;
    }
}