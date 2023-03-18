namespace Rydo.Storage.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal static class TaskExtensions
    {
        public static async Task<T> TimeoutAfter<T>(this Task<T> task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                var taskDelay = Task.Delay(timeout, cts.Token);
                var resultTask = await Task.WhenAny(task, taskDelay);
                if (resultTask == taskDelay)
                {
                    throw new OperationCanceledException();
                }
                else
                {
                    cts.Cancel();
                }
            }

            return await task;
        }

        public static async Task TimeoutAfter(this Task task, TimeSpan timeout)
        {
            using (var cts = new CancellationTokenSource())
            {
                var taskDelay = Task.Delay(timeout, cts.Token);
                var resultTask = await Task.WhenAny(task, taskDelay);
                if (resultTask == taskDelay)
                {
                    throw new OperationCanceledException();
                }
                else
                {
                    cts.Cancel();
                }
            }

            await task;
        }
        
        public static async ValueTask<T> WaitCompletedSuccessfully<T>(this Task<T> task) =>
            task.IsCompletedSuccessfully ? task.Result : await task;

        public static async ValueTask<T> WaitCompletedSuccessfully<T>(this ValueTask<T> task) =>
            task.IsCompletedSuccessfully ? task.Result : await task;

        public static ValueTask WaitCompletedSuccessfully(this Task task)
        {
            return task.IsCompletedSuccessfully ? new ValueTask(Task.CompletedTask) : SlowTask(task);

            static async ValueTask SlowTask(Task task) => await task;
        }

        public static ValueTask WaitCompletedSuccessfully(this ValueTask task)
        {
            return task.IsCompletedSuccessfully ? new ValueTask(Task.CompletedTask) : SlowTask(task);

            static async ValueTask SlowTask(ValueTask task) => await task;
        }
    }
}