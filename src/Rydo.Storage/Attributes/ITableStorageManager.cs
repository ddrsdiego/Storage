namespace Rydo.Storage.Attributes
{
    public interface ITableStorageManager
    {
        bool TryExtractTopicName(object? model, out string? tableName);
    }
}