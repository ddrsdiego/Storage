namespace Rydo.Storage.UnitTest.Read
{
    using System.Text.Json;
    using FluentAssertions;
    using Storage.Providers;
    using Storage.Read;
    using Write;
    using Xunit;

    public class ReadRequestTest
    {
        [Fact]
        public void Test()
        {
            var readRequest = new ReadRequest("5090016", new ModelTypeDefinition(typeof(DummyModel)),
                FutureReadResponse.GetInstance());
            var writeRequest = WriteRequest
                .Builder(WriteRequestOperation.Upsert, FutureWriteResponse.GetInstance())
                .WithKey("5090016")
                .WithPayload(JsonSerializer.SerializeToUtf8Bytes(new DummyModel {AccountNumber = "5090016"}))
                .Build();

            var storageItem = writeRequest.ToStorageItem();
            var storageKey = readRequest.ToStorageItemKey();

            storageItem.Key.Value.Should().Be(storageKey.Value);
        }
    }
}