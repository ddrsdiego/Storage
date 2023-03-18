namespace Rydo.Storage.Serialization
{
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    public class RydoStorageCacheSystemTextJsonSerializer :
        IRydoStorageCacheSerializer
    {
        private readonly JsonSerializerOptions _options;

        public RydoStorageCacheSystemTextJsonSerializer()
            :this(new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            })
        {
        }

        private RydoStorageCacheSystemTextJsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        public byte[] Serialize<T>(T obj) => JsonSerializer.SerializeToUtf8Bytes(obj, _options);

        public T Deserialize<T>(byte[] data) => JsonSerializer.Deserialize<T>(data, _options);

        public async ValueTask<byte[]> SerializeAsync<T>(T obj)
        {
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync<T>(stream, obj, _options);
            
            return stream.ToArray();
        }

        public async ValueTask<T> DeserializeAsync<T>(byte[] data)
        {
            using var stream = new MemoryStream(data);
            return await JsonSerializer.DeserializeAsync<T>(stream, _options);
        }
    }
}