namespace Rydo.Storage.Redis
{
    public struct RedisConfiguration
    {
        public string? WriteEndpoint { get; set; }
        public string? ReadeEndpoint { get; set; }
        public string? DbInstance { get; set; }
    }
}