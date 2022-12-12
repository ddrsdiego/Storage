namespace Rydo.Storage.Helpers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IFuture
    {
        Task Run();
    }

    internal class Future<T> : IFuture
    {
        private readonly CancellationToken _cancellationToken;
        private readonly TaskCompletionSource<T> _completion;
        private readonly Func<Task<T>> _method;

        public Future(Func<Task<T>> method, CancellationToken cancellationToken)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _cancellationToken = cancellationToken;
            _completion = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
        }

        /// <summary>
        /// The post-execution result, which can be awaited
        /// </summary>
        public Task<T> Completed => _completion.Task;

        public async Task Run()
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                _completion.TrySetCanceled(_cancellationToken);
                return;
            }

            try
            {
                var result = await _method().ConfigureAwait(false);

                _completion.TrySetResult(result);
            }
            catch (OperationCanceledException exception) when (exception.CancellationToken == _cancellationToken)
            {
                _completion.TrySetCanceled(exception.CancellationToken);
            }
            catch (Exception exception)
            {
                _completion.TrySetException(exception);
            }
        }
    }
}