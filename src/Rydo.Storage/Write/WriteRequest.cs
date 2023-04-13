namespace Rydo.Storage.Write
{
    using System;
    using System.Buffers;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;
    using Helpers;
    using Providers;

    public readonly struct WriteRequest
    {
        internal WriteRequest(WriteRequestOperation operation, IModelTypeDefinition modelTypeDefinition, string key,
            string sortKey, byte[] payload, FutureWriteResponse response)
        {
            ModelTypeDefinition = modelTypeDefinition;
            Key = key ?? throw new ArgumentNullException(nameof(key));

            if (operation == WriteRequestOperation.Upsert && payload == null)
                throw new ArgumentNullException(nameof(payload));
            
            OperationId = GeneratorOperationId.Generate();
            Payload = payload;
            SortKey = sortKey;
            Response = response;
            Operation = operation;
            RequestedAt = DateTime.Now;
        }

        public readonly string OperationId;
        public readonly string Key;
        public readonly string SortKey;
        public readonly byte[] Payload;
        public readonly DateTime RequestedAt;

        internal readonly FutureWriteResponse Response;
        internal readonly WriteRequestOperation Operation;
        internal readonly IModelTypeDefinition ModelTypeDefinition;

        public T GetRaw<T>()
        {
            var reader = new Utf8JsonReader(new ReadOnlySequence<byte>(Payload));
            return JsonSerializer.Deserialize<T>(ref reader);
        }

        public async ValueTask<T> GetRawAsync<T>()
        {
            using var stream = new MemoryStream(Payload);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }

        internal static WriteRequestBuilder Builder(WriteRequestOperation operation,
            FutureWriteResponse futureWriteResponse) => new WriteRequestBuilder(operation, futureWriteResponse);

        public bool IsValidRequest => !string.IsNullOrEmpty(Key);

        public bool IsInvalidRequest => !IsValidRequest;

        internal StorageItem ToStorageItem() => new StorageItem(Key, SortKey, Payload);
    }
}