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
            const string accountNumber = "5090016";
            
            var readRequest = new ReadRequest(accountNumber, new ModelTypeDefinition(typeof(DummyModel)),
                FutureReadResponse.GetInstance());

            var payload = JsonSerializer.SerializeToUtf8Bytes(new DummyModel {AccountNumber = accountNumber});
            var writeRequest = WriteRequest
                .Builder(WriteRequestOperation.Upsert, FutureWriteResponse.GetInstance())
                .WithKey(accountNumber)
                .WithPayload(payload)
                .Build();

            var storageItem = writeRequest.ToStorageItem();
            var storageKey = readRequest.ToStorageItemKey();

            storageItem.Key.Value.Should().Be(storageKey.Value);
        }
    }
}