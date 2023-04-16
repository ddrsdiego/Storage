﻿namespace Rydo.Storage.Read.v2
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

    internal sealed class StorageReaderConsumer : IStorageReaderConsumer
    {
        private const int Capacity = 1024 * 1024;

        private readonly Task _readerTask;
        private readonly IStorageRead _storageRead;
        private readonly Channel<ReadRequest> _queue;
        private readonly CancellationToken _cancellationToken;
        private readonly ModelTypeContext _modelTypeContext;
        private readonly TaskCompletionSource<bool> _consumerTaskRunner;

        public StorageReaderConsumer(ILoggerFactory logger,
            ModelTypeContext modelTypeContext,
            IStorageRead storageRead,
            IServiceProvider serviceProvider)
        {
            _modelTypeContext = modelTypeContext;
            _storageRead = storageRead;
            var channelOptions = new BoundedChannelOptions(Capacity)
            {
                AllowSynchronousContinuations = true,
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = true,
                SingleWriter = false
            };

            _queue = Channel.CreateBounded<ReadRequest>(channelOptions);

            _cancellationToken = serviceProvider
                .GetRequiredService<IHostApplicationLifetime>()
                .ApplicationStopping;

            _consumerTaskRunner = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            _readerTask = Task.Run(async () => await ReadFromChannel());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ValueTask EnqueueRequest(ReadRequest readRequest,
            CancellationToken cancellationToken = default)
        {
            var writeTask = _queue.Writer.WriteAsync(readRequest, _cancellationToken);

            var result = writeTask.IsCompletedSuccessfully
                ? new ValueTask(Task.CompletedTask)
                : SlowWrite(writeTask);

            Internals.Metrics.Gauges.ReadRequestInQueue.Inc();

            return result;

            static async ValueTask SlowWrite(ValueTask task) => await task;
        }

        private async Task ReadFromChannel()
        {
            var batchCapacity = _modelTypeContext.Definition.ReadBufferSize;

            try
            {
                while (!_cancellationToken.IsCancellationRequested)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        break;

                    while (await _queue.Reader.WaitToReadAsync())
                    {
                        var counter = 0;
                        var batch = new ReadBatchRequest(batchCapacity);

                        while (counter < batchCapacity && _queue.Reader.TryRead(out var readRequest))
                        {
                            batch.TryAdd(readRequest);
                            counter++;

                            Internals.Metrics.Gauges.ReadRequestInQueue.Dec();
                        }

                        var readTask = _storageRead.Execute(batch, _cancellationToken);

                        await readTask.WaitCompletedSuccessfully();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (_consumerTaskRunner.TrySetResult(false))
                {
                }
            }
        }
    }
}