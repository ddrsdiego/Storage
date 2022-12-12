namespace Rydo.Storage.Read
{
    using System;
    using System.Runtime.CompilerServices;
    using Providers;

    public readonly struct ReadRequest
    {
        internal ReadRequest(string key, IModelTypeDefinition definition, FutureReadResponse futureReadResponse)
            : this(key, string.Empty, definition, futureReadResponse)
        {
        }

        internal ReadRequest(string key, string sortKey, IModelTypeDefinition definition, FutureReadResponse response)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            Response = response;
            Definition = definition;
            SortKey = sortKey;
            RequestAt = DateTime.Now;
        }

        public readonly string? Key;
        public readonly string? SortKey;
        public readonly IModelTypeDefinition Definition;
        public readonly FutureReadResponse Response;
        public readonly DateTime RequestAt;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal StorageItemKey ToStorageItemKey() => new StorageItemKey(Key, SortKey);
    }
}