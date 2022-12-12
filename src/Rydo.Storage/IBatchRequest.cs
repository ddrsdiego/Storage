namespace Rydo.Storage
{
    public interface IBatchRequest
    {
        string BatchId { get; }

        int Count { get; }
    }
}