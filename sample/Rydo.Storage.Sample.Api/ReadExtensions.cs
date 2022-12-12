namespace Rydo.Storage.Sample.Api
{
    using System.Net.Mime;
    using System.Text.Json;
    using Read;

    internal static class ReadExtensions
    {
        public static async ValueTask WriteResponseAsync(this Task<ReadResponse> readResponse, HttpResponse response)
        {
            var readResult = await readResponse;

            response.StatusCode = (int) readResult.StatusCode;
            response.ContentType = MediaTypeNames.Application.Json;

            await response.StartAsync();

            if (readResult.StatusCode == ReadResponseStatus.Ok)
                await response.BodyWriter.WriteAsync(readResult.Value);
            else
                await response.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(new { })));

            await response.BodyWriter.CompleteAsync();
        }
    }
}