namespace Rydo.Storage.Sample.Api
{
    using System.IO.Pipelines;
    using System.Net.Mime;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.Json;
    using Read;

    internal static class ReadExtensions
    {
        public static async ValueTask WriteResponseAsync(this ValueTask<ReadResponse> readResponse,
            HttpResponse response)
        {
            var readResult = await readResponse;

            response.StatusCode = (int) readResult.StatusCode;
            response.ContentType = MediaTypeNames.Application.Json;

            await response.StartAsync();

            if (readResult.StatusCode == ReadResponseStatus.Ok)
            {
                response.BodyWriter.WriteToPipe(readResult.Value, Encoding.UTF8.GetEncoder());
                // await response.BodyWriter.WriteAsync(readResult.Value);
            }
            else
                await response.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(new { })));

            await response.BodyWriter.CompleteAsync();
        }

        public static async ValueTask WriteResponseAsync(this Task<ReadResponse> readResponse, HttpResponse response)
        {
            var readResult = await readResponse;

            response.StatusCode = (int) readResult.StatusCode;
            response.ContentType = MediaTypeNames.Application.Json;

            await response.StartAsync();

            if (readResult.StatusCode == ReadResponseStatus.Ok)
            {
                response.BodyWriter.WriteToPipe(readResult.Value, Encoding.UTF8.GetEncoder());
                // await response.BodyWriter.WriteAsync(readResult.Value);
            }
            else
                await response.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(new { })));

            await response.BodyWriter.CompleteAsync();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteToPipe(this IReadOnlyList<byte> value, PipeWriter pipe) => value.WriteToPipe(pipe, Encoding.UTF8.GetEncoder());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteToPipe(this IReadOnlyList<byte> value, PipeWriter pipe, Encoder encoder)
        {
            Span<char> charSpan = stackalloc char[value.Count];
            for (var counter = 0; counter < value.Count; counter++)
            {
                charSpan[counter] = (char) value[counter];
            }

            var bytesNeeded = encoder.GetByteCount(charSpan, true);
            var bytesWritten = encoder.GetBytes(charSpan, pipe.GetSpan(bytesNeeded), true);

            pipe.Advance(bytesWritten);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteToPipe(this PipeWriter pipe, IReadOnlyList<byte> value, Encoder encoder)
        {
            Span<char> charSpan = stackalloc char[value.Count];
            for (var counter = 0; counter < value.Count; counter++)
            {
                charSpan[counter] = (char) value[counter];
            }

            var bytesNeeded = encoder.GetByteCount(charSpan, true);
            var bytesWritten = encoder.GetBytes(charSpan, pipe.GetSpan(bytesNeeded), true);

            pipe.Advance(bytesWritten);
        }
    }
}