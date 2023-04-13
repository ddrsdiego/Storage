namespace Rydo.Storage.Attributes
{
    public interface ITableStorageManager
    {
        bool TryExtractTableName(object model, out string tableName);
    }
}