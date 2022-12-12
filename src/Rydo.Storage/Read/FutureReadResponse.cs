namespace Rydo.Storage.Read
{
    using System.Threading.Tasks;

    public readonly struct FutureResponse<TResponse>
    {
        private readonly TaskCompletionSource<TResponse> _taskCompletion;

        private FutureResponse(TaskCompletionSource<TResponse> taskCompletion) => _taskCompletion = taskCompletion;

        public Task<TResponse> ReadTask => _taskCompletion.Task;

        public static FutureResponse<TResponse> GetInstance() =>
            new FutureResponse<TResponse>(new TaskCompletionSource<TResponse>(TaskCreationOptions.RunContinuationsAsynchronously));

        internal ValueTask TrySetResult(TResponse response)
        {
            _taskCompletion.TrySetResult(response);
            return new ValueTask(Task.CompletedTask);
        }
    }

    public readonly struct FutureReadResponse
    {
        private readonly TaskCompletionSource<ReadResponse> _taskCompletion;

        private FutureReadResponse(TaskCompletionSource<ReadResponse> taskCompletion) =>
            _taskCompletion = taskCompletion;

        public Task<ReadResponse> ReadTask => _taskCompletion.Task;

        public static FutureReadResponse GetInstance() => new FutureReadResponse(
            new TaskCompletionSource<ReadResponse>(TaskCreationOptions.RunContinuationsAsynchronously));

        internal ValueTask TrySetResult(ReadResponse response)
        {
            _taskCompletion.TrySetResult(response);
            return new ValueTask(Task.CompletedTask);
        }
    }
}