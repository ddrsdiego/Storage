namespace Rydo.Storage.Write
{
    using System.Threading.Tasks;

    public readonly struct FutureWriteResponse
    {
        private readonly TaskCompletionSource<WriteResponse> _taskCompletion;

        private FutureWriteResponse(TaskCompletionSource<WriteResponse> taskCompletion)
        {
            _taskCompletion = taskCompletion;
        }

        public Task<WriteResponse> WriteTask => _taskCompletion.Task;

        public static FutureWriteResponse GetInstance() => new FutureWriteResponse(
            new TaskCompletionSource<WriteResponse>(TaskCreationOptions.RunContinuationsAsynchronously));

        internal ValueTask TrySetResult(WriteResponse response)
        {
            _taskCompletion.TrySetResult(response);
            return new ValueTask(Task.CompletedTask);
        }
    }
}