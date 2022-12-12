namespace Rydo.Storage.UnitTest.Providers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Storage.Providers;
    using Storage.Read;
    using Write;

    public class FakeStorageContentProvider : IStorageContentProvider
    {
        public Task Read(ReadBatchRequest batch, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task Read(ReadBatchRequest batch, Func<ReadBatchRequest, StorageItem, ValueTask> onStorageRead)
        {
            throw new NotImplementedException();
        }

        public async Task Write(IWriteBatchRequest writeBatchRequest, CancellationToken cancellationToken = default)
        {
            foreach (var writeRequest in writeBatchRequest)
            {
                var response = WriteResponse.GetCreatedInstance(writeRequest);

                await writeRequest.Response.TrySetResult(response);
            }

            await Task.Delay(5_000, cancellationToken);
        }
    }
}