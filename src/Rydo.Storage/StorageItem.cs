namespace Rydo.Storage
{
    using System;

    public readonly struct StorageItem
    {
        public StorageItem(string key, string sortKey, byte[] payload)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            Key = new StorageItemKey(key, sortKey);
            Payload = payload;
            CreatedAt = DateTime.Now;
        }

        public readonly StorageItemKey Key;
        public readonly ReadOnlyMemory<byte> Payload;
        public readonly DateTime CreatedAt;
    }
}