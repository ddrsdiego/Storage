namespace Rydo.Storage.Benchmark
{
    using BenchmarkDotNet.Running;

    public static class Program
    {
        public static void Main(string[] args)
        {
            // var helper = new WebClientHelper();
            //
            // var a = helper.DownloadString();

            BenchmarkRunner.Run<WebClientHelper>();
        }
    }
}