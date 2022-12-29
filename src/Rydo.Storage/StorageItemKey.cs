namespace Rydo.Storage
{
    using System;

    public readonly struct StorageItemKey
    {
        public StorageItemKey(string key)
            : this(key, string.Empty)
        {
        }

        public StorageItemKey(string key, string sortKey)
        {
            Key = key;
            SortKey = sortKey;
        }

        public bool IsComposed => !string.IsNullOrEmpty(Key) && !string.IsNullOrEmpty(SortKey);

        public readonly string Key;
        public readonly string SortKey;
        public string Value => SanitizeKey();

        internal string SanitizeKey() => string.IsNullOrEmpty(SortKey) ? Key : $"{Key}-{SortKey}";

        public override bool Equals(object obj)
        {
            var other = (StorageItemKey) obj;
            return other.GetHashCode().Equals(GetHashCode());
        }

        public override int GetHashCode() => HashCode.Combine(Key, SortKey);
    }
}