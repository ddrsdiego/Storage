namespace Rydo.Storage.Internals.Metrics
{
    using Prometheus;

    internal static class Gauges
    {
        public static readonly Gauge WriteRequestInQueue = Metrics
            .CreateGauge("write_request_enqueue", "Number of jobs waiting for processing in the queue.");

        public static readonly Gauge ReadRequestInQueue = Metrics
            .CreateGauge("read_request_enqueue", "Number of jobs waiting for processing in the queue.");
    }
}