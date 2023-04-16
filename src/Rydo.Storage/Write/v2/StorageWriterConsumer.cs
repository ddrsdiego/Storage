namespace Rydo.Storage.Write.v2
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Providers;

    internal sealed class StorageWriterConsumer : IStorageWriterConsumer
    {
        private const int Capacity = 1024 * 1024;

        private readonly Task _readerTask;
        private readonly IStorageWrite _storageWrite;
        private readonly Channel<WriteRequest> _queue;
        private readonly ModelTypeContext _modelTypeContext;
        private readonly CancellationToken _cancellationToken;
        private readonly ILogger<StorageWriterConsumer> _logger;
        private readonly TaskCompletionSource<bool> _taskCompletion;

        public StorageWriterConsumer(ILoggerFactory logger,
            ModelTypeContext modelTypeContext,
            IStorageWrite storageWrite,
            IServiceProvider serviceProvider)
        {
            _logger = logger.CreateLogger<StorageWriterConsumer>();
            _modelTypeContext = modelTypeContext;
            _storageWrite = storageWrite;

            var channelOptions = new BoundedChannelOptions(Capacity)
            {
                AllowSynchronousContinuations = true,
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            };

            _queue = Channel.CreateBounded<WriteRequest>(channelOptions);

            _cancellationToken = serviceProvider
                .GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopping;

            _taskCompletion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _readerTask = Task.Run(ReadFromChannel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask EnqueueRequest(WriteRequest writeRequest)
        {
            var writeTask = _queue.Writer.WriteAsync(writeRequest, _cancellationToken);

            var taskResult = writeTask.IsCompletedSuccessfully
                ? new ValueTask(Task.CompletedTask)
                : SlowWrite(writeTask);

            Internals.Metrics.Gauges.WriteRequestInQueue.Inc();

            return taskResult;

            static async ValueTask SlowWrite(ValueTask task) => await task;
        }

        private async Task TryTerminateChannel()
        {
            _queue.Writer.Complete();
            await _taskCompletion.Task;
        }

        private async Task ReadFromChannel()
        {
            var batchCapacity = _modelTypeContext.Definition.WriteBufferSize;

            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        break;

                    while (await _queue.Reader.WaitToReadAsync())
                    {
                        var counter = 0;
                        IWriteBatchRequest batch = new WriteBatchRequest(batchCapacity);

                        while (counter < batchCapacity && _queue.Reader.TryRead(out var writeRequest))
                        {
                            batch.TryAdd(writeRequest);
                            counter++;

                            Internals.Metrics.Gauges.WriteRequestInQueue.Dec();
                        }

                        var writeTask = _storageWrite.Write(batch, _cancellationToken);

                        await writeTask.WaitCompletedSuccessfully();
                    }
                }

                await TryTerminateChannel();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "");
            }
            finally
            {
                if (_taskCompletion.TrySetResult(false))
                {
                }
            }
        }
    }
}