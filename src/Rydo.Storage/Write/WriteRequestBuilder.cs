namespace Rydo.Storage.Write
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.Json;
    using Providers;

    internal sealed class WriteRequestBuilder
    {
        private readonly WriteRequestOperation _operation;
        private readonly FutureWriteResponse _futureWriteResponse;
        private string? _key;
        private string? _shortKey;
        private byte[]? _payload;
        private IModelTypeDefinition? _modelTypeDefinition;

        public WriteRequestBuilder(WriteRequestOperation operation, FutureWriteResponse futureWriteResponse)
        {
            _operation = operation;
            _futureWriteResponse = futureWriteResponse;
        }

        public WriteRequest Build() => new WriteRequest(_operation, _modelTypeDefinition, _key, _shortKey,
            _payload, _futureWriteResponse);

        public WriteRequestBuilder WithKey(string key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
            return this;
        }

        public WriteRequestBuilder WithSortKey(string? shortKey)
        {
            _shortKey = shortKey;
            return this;
        }

        public WriteRequestBuilder WithPayload<T>([DisallowNull] T payload)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));

            WithPayload(JsonSerializer.SerializeToUtf8Bytes(payload));
            return this;
        }

        public WriteRequestBuilder WithPayload(byte[] payload)
        {
            _payload = payload ?? throw new ArgumentNullException(nameof(payload));
            return this;
        }

        public WriteRequestBuilder WithModelTypeDefinition(IModelTypeDefinition modelTypeDefinition)
        {
            _modelTypeDefinition = modelTypeDefinition;
            return this;
        }
    }
}