namespace Rydo.Storage.UnitTest.Writer
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Write;
    using Xunit;
    using Xunit.Abstractions;

    public class WriteBatchRequestTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public WriteBatchRequestTest(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

        [Fact]
        public void SyncMethodsTest()
        {
            _testOutputHelper.WriteLine(
                $"Start All-> {nameof(GetNumberOneAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");

            var task1 = GetNumberOne();
            var task2 = GetNumberTwo();

            _testOutputHelper.WriteLine(
                $"Get All Tasks -> {nameof(GetNumberOneAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");

            _testOutputHelper.WriteLine(
                $"Finish All -> {nameof(GetNumberOneAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");
        }

        [Fact]
        public void TaskRunWithExceptionExample()
        {
            _testOutputHelper.WriteLine($"Starting --> {DateTime.Now.ToLongTimeString()}");

            var task = Task.Run(() =>
            {
                _testOutputHelper.WriteLine($"Running Task --> {DateTime.Now.ToLongTimeString()}");
                // throw new NullReferenceException("NullReferenceException on the Running Task");

                return 0;
            });

            _testOutputHelper.WriteLine($"Finishing --> {DateTime.Now.ToLongTimeString()}");
        }

        [Fact]
        public void TaskRunExample()
        {
            _testOutputHelper.WriteLine($"Starting --> {DateTime.Now.ToLongTimeString()}");

            var task = Task.Run(() =>
            {
                //This code will be executed on a ThreadPool
                var result = 0;

                for (var i = 0; i < 20; i++)
                {
                    result += 1;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(1_000));
                return result;
            });

            _testOutputHelper.WriteLine(
                $"Status --> {task.Status} - {DateTime.Now.ToLongTimeString()}");

            _testOutputHelper.WriteLine(
                $"Result --> {task.Result} - {DateTime.Now.ToLongTimeString()}"); //This line waits (block the main thread)

            _testOutputHelper.WriteLine(
                $"Status --> {task.Status} - {DateTime.Now.ToLongTimeString()}");

            _testOutputHelper.WriteLine($"Finishing --> {DateTime.Now.ToLongTimeString()}");
        }

        [Fact]
        public void AsyncMethodsTest()
        {
            _testOutputHelper.WriteLine($"Starting --> {DateTime.Now.ToLongTimeString()}");

            var taskConstructor = new Task(() => DoSomeWork("Constructor"));
            var taskFactoryStartNew = Task.Factory.StartNew(() => DoSomeWork("Task.Factory.StartNew"));
            var taskRun = Task.Run(() => DoSomeWork("Task.Run"));

            var task = Task.Delay(TimeSpan.FromMilliseconds(1_000));
            task.Wait();

            taskFactoryStartNew.Wait();
            taskRun.Wait();
            taskConstructor.Start();

            _testOutputHelper.WriteLine($"Finishing --> {DateTime.Now.ToLongTimeString()}");

            // _testOutputHelper.WriteLine(
            //     $"Start All-> {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");
            //
            // var downloadTask = DownloadAsync();
            //
            // var task1 = GetNumberOneAsync();
            // var task2 = await GetNumberTwoAsync();
            // var res = await downloadTask;
            //
            // _testOutputHelper.WriteLine(
            //     $"Get All Tasks -> {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");
            //
            // _testOutputHelper.WriteLine(
            //     $"Finish All -> {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");
        }

        private void DoSomeWork(in string workType)
        {
            _testOutputHelper.WriteLine($"{workType} - {DateTime.Now.ToLongTimeString()}");
        }

        private static string DownloadAsyncCallback(Task<string> task)
        {
            if (!task.IsCompletedSuccessfully)
            {
            }

            return task.Result;
        }

        private static Task<string> DownloadAsync()
        {
            var client = new HttpClient();

            var getStringTask =
                client.GetStringAsync("https://docs.microsoft.com/dotnet");

            return getStringTask;
        }

        private int GetNumberOne()
        {
            const int numberOne = 1;

            _testOutputHelper.WriteLine(
                $"Start -> {nameof(GetNumberOne)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");

            Thread.Sleep(TimeSpan.FromSeconds(numberOne));

            _testOutputHelper.WriteLine(
                $"Finish -> {nameof(GetNumberOne)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");

            return numberOne;
        }

        private int GetNumberTwo()
        {
            const int numberTwo = 2;

            _testOutputHelper.WriteLine(
                $"Start -> {nameof(GetNumberTwo)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");

            Thread.Sleep(TimeSpan.FromSeconds(numberTwo));

            _testOutputHelper.WriteLine(
                $"Finish -> {nameof(GetNumberTwo)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Thread.GetCurrentProcessorId()}");

            return numberTwo;
        }

        private async Task<int> GetNumberOneAsync()
        {
            const int numberOne = 1;

            _testOutputHelper.WriteLine(
                $"Start -> {nameof(GetNumberOneAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");

            await Task.Delay(TimeSpan.FromSeconds(numberOne));

            _testOutputHelper.WriteLine(
                $"Finish -> {nameof(GetNumberOneAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");

            throw new Exception();
            return numberOne;
        }

        private async Task<int> GetNumberTwoAsync()
        {
            const int numberTwo = 2;

            _testOutputHelper.WriteLine(
                $"Start -> {nameof(GetNumberTwoAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");

            await Task.Delay(TimeSpan.FromSeconds(numberTwo));

            _testOutputHelper.WriteLine(
                $"Finish -> {nameof(GetNumberTwoAsync)} - {DateTime.Now.ToString(CultureInfo.InvariantCulture)} - {Environment.CurrentManagedThreadId}");

            return numberTwo;
        }

        private static void AddOrUpdate(string value)
        {
            throw new Exception("Bla Bla Bla");
        }

        [Fact]
        public void Should_Create_BatchRequest_With_One_Item()
        {
            var writeBatchRequest = new WriteBatchRequest(1);

            var futureResponse = FutureWriteResponse.GetInstance();
            var request = WriteRequest
                .Builder(WriteRequestOperation.Upsert, futureResponse)
                .WithKey("5090016")
                .WithPayload<DummyModel>(new DummyModel {AccountNumber = "5090016"})
                .Build();

            writeBatchRequest.TryAdd(request);

            writeBatchRequest.Count.Should().Be(1);
        }

        [Fact]
        public async Task Should_Create_BatchRequest_With_One_Item_And_SortKey()
        {
            const string key = "5090016";
            const string sortKey = "SELIC";
            const string storageKey = $"{key}-{sortKey}";

            var writeBatchRequest = new WriteBatchRequest(1);

            var futureResponse = FutureWriteResponse.GetInstance();
            var request = WriteRequest
                .Builder(WriteRequestOperation.Upsert, futureResponse)
                .WithKey(key)
                .WithSortKey(sortKey)
                .WithPayload(new DummyModel {AccountNumber = "5090016"})
                .Build();

            writeBatchRequest.TryAdd(request);

            writeBatchRequest.Count.Should().Be(1);
            writeBatchRequest.Count(x => x.Key!.Equals(key, StringComparison.InvariantCultureIgnoreCase)).Should()
                .Be(1);

            writeBatchRequest.StorageItems
                .Count(x => x.Key.Value!.Equals(storageKey, StringComparison.InvariantCultureIgnoreCase)).Should()
                .Be(1);

            foreach (var writeRequest in writeBatchRequest)
            {
                await writeRequest.Response.TrySetResult(WriteResponse.GetCreatedInstance(writeRequest));
            }

            var response = await writeBatchRequest.First().Response.WriteTask;

            response.Request.Key.Should().Be(key);
            response.Request.SortKey.Should().Be(sortKey);
            response.Request.Operation.Should().Be(WriteRequestOperation.Upsert);
        }
    }
}