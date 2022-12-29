namespace Rydo.Storage.Attributes
{
    using System.Collections.Immutable;
    using System.Runtime.CompilerServices;
    using Microsoft.Extensions.Logging;

    public sealed class TableStorageManager : ITableStorageManager
    {
        private readonly object _lockObject;
        private readonly ILogger<TableStorageManager> _logger;
        private ImmutableDictionary<string, string> _cache;

        public TableStorageManager(ILogger<TableStorageManager> logger)
        {
            _logger = logger;
            _lockObject = new object();
            _cache = ImmutableDictionary<string, string>.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryExtractTopicName(object model, out string tableName) =>
            ExecuteTryExtractTopicName(model, out tableName);

        private bool ExecuteTryExtractTopicName(object model, out string tableName)
        {
            tableName = string.Empty;

            if (!model.TryExtractTableName(out var tableNameFromModel))
                return false;

            var modeFullName = model?.GetType().FullName;

            if (_cache.TryGetValue(modeFullName!, out tableName))
                return true;
            
            lock (_lockObject)
            {
                if (_cache.TryGetValue(modeFullName!, out tableName))
                    return true;

                _cache = _cache.Add(modeFullName!, tableNameFromModel);
                tableName = tableNameFromModel;
            }

            return true;
        }
    }
}