namespace Rydo.Storage.Benchmark
{
    using System.Net;
    using BenchmarkDotNet.Attributes;

    [MemoryDiagnoser]
    public class WebClientHelper    
    {
        private const string Address = "https://google.com/";
        
        private readonly WebClient _client;

        public WebClientHelper()
        {
            _client = new WebClient();
        }

        [Benchmark]
        public string DownloadString()
        {
            // Thread.Sleep(TimeSpan.FromMilliseconds(100));
            return "Test";
        }

        // [Benchmark]
        // public string DownloadStringWithWait()
        // {
        //     var task = _client.DownloadStringTaskAsync(Address);
        //     return task.GetAwaiter().GetResult();
        // }
        //
        // [Benchmark]
        // public string DownloadStringWithResult()
        // {
        //     var task = _client.DownloadStringTaskAsync(Address);
        //     return task.Result;
        // }
        //
        // [Benchmark]
        // public async Task<string> DownloadStringWithAsyncAwait()
        // {
        //     return await _client.DownloadStringTaskAsync(Address);
        // }
    }
}