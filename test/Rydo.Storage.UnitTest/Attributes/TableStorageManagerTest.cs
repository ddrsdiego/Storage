namespace Rydo.Storage.UnitTest.Attributes
{
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Storage.Attributes;
    using Xunit;

    public class TableStorageManagerTest
    {
        private readonly ILogger<TableStorageManager> _logger;

        public TableStorageManagerTest()
        {
            _logger = NSubstitute.Substitute.For<ILogger<TableStorageManager>>();
        }

        [Fact]
        public void Should_Extract_TableName_From_Model()
        {
            const string tableNameModel = "dummy-model";

            var model = new DummyModel { };
            var sut = new TableStorageManager(_logger);

            var res = sut.TryExtractTableName(model, out var tableName);

            res.Should().BeTrue();
            tableName.Should().Be(tableNameModel);
        }
    }
}