namespace Rydo.Storage.UnitTest
{
    using FluentAssertions;
    using Xunit;

    public class StorageItemKeyTest
    {
        [Fact]
        public void Should_Create_Valid_StorageItemKey()
        {
            const string? key = "5090016";

            var storageItemKey1 = new StorageItemKey(key);
            var storageItemKey2 = new StorageItemKey(key);

            storageItemKey1.Value.Should().Be(key);
            storageItemKey1.Should().Be(storageItemKey2);
        }

        [Fact]
        public void Should_Create_Valid_StorageItemKey_With_SortKey()
        {
            const string? key = "5090016";
            const string? sortKey = "SELIC";

            var storageItemKey1 = new StorageItemKey(key, sortKey);
            var storageItemKey2 = new StorageItemKey(key, sortKey);

            var value = storageItemKey1.SanitizeKey();
            
            storageItemKey1.Value.Should().Be(value);
            storageItemKey1.Should().Be(storageItemKey2);
            storageItemKey1.IsComposed.Should().BeTrue();
        }
    }
}