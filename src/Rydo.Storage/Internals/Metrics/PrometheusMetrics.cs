namespace Rydo.Storage.Internals.Metrics
{
    internal static class PrometheusMetrics
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tableName"></param>
        public static void ReadRequestNotFound(string key, string tableName) =>
            Counters.ReadRequestNotFound.WithLabels(key, tableName).Inc();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="timeLimit"></param>
        /// <param name="elapsedExceeded"></param>
        public static void ReadRequestElapsedTimeExceeded(string? tableName, int timeLimit) => Counters
            .ReadRequestElapsedTimeExceeded.WithLabels(tableName, timeLimit.ToString()).Inc();
    }
}