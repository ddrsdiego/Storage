namespace Rydo.Storage.Internals.Metrics
{
    using Prometheus;

    internal static class Counters
    {
        public static readonly Counter ReadRequestNotFound =
            Metrics.CreateCounter("read_request_not_found", "Number of not found requests for a given key.",
                new CounterConfiguration
                {
                    LabelNames = new[] {PrometheusLabels.Key, PrometheusLabels.TableName}
                });

        public static readonly Counter ReadRequestElapsedTimeExceeded =
            Metrics.CreateCounter("read_request_elapsed_time_exceeded",
                "Number of calls that exceeded the request time limit.",
                new CounterConfiguration
                {
                    LabelNames = new[]
                    {
                        PrometheusLabels.TableName,
                        PrometheusLabels.TimeLimit
                    }
                });
    }
}