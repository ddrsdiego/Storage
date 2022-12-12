namespace Rydo.Storage.UnitTest.Read
{
    using FluentAssertions;
    using Storage.Providers;
    using Storage.Read;
    using Xunit;

    public class ReadBatchRequestTest
    {
        [Fact]
        public void Should_Create_BatchRequest_With_Many_Items()
        {
            IReadBatchRequest readBatchRequest = new ReadBatchRequest(3);

            const string accountNumber = "5090016";
            const string sortKey = "SELIC";

            var readRequest1 = new ReadRequest(accountNumber, new ModelTypeDefinition(typeof(DummyModel)),
                FutureReadResponse.GetInstance());

            var readRequest2 = new ReadRequest(accountNumber, sortKey, new ModelTypeDefinition(typeof(DummyModel)),
                FutureReadResponse.GetInstance());

            var readRequest3 = new ReadRequest(accountNumber, new ModelTypeDefinition(typeof(DummyModel)),
                FutureReadResponse.GetInstance());

            readBatchRequest.TryAdd(readRequest1);
            readBatchRequest.TryAdd(readRequest2);
            readBatchRequest.TryAdd(readRequest3);

            readBatchRequest.Count.Should().Be(3);
        }
    }
}