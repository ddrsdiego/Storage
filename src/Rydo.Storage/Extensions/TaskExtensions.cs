namespace Rydo.Storage.Extensions
{
    using System.Threading.Tasks;

    internal static class TaskExtensions
    {
        public static async ValueTask<T> WaitCompletedSuccessfully<T>(this Task<T> task)
        {
            if (!task.IsCompletedSuccessfully)
                return await task;

            return task.Result;
        }

        public static async ValueTask<T> WaitCompletedSuccessfully<T>(this ValueTask<T> task)
        {
            if (!task.IsCompletedSuccessfully)
                return await task;

            return task.Result;
        }

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