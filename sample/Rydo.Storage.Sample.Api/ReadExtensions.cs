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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskReadResponse"></param>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        public static async Task WriteToPipeAsync(this ValueTask<ReadResponse> taskReadResponse,
            HttpResponse response, CancellationToken cancellationToken = default)
        {
            var readResponse = await taskReadResponse;
            await readResponse.WriteToPipeAsync(response, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskReadResponse"></param>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        public static async Task WriteToPipeAsync(this Task<ReadResponse> taskReadResponse, HttpResponse response,
            CancellationToken cancellationToken = default)
        {
            var readResponse = await taskReadResponse;
            await readResponse.WriteToPipeAsync(response, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readResponse"></param>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task WriteToPipeAsync(this ReadResponse readResponse, HttpResponse response,
            CancellationToken cancellationToken = default)
        {
            response.StatusCode = (int) readResponse.StatusCode;
            response.ContentType = MediaTypeNames.Application.Json;

            await response.StartAsync(cancellationToken);

            if (readResponse.StatusCode == ReadResponseStatus.Ok)
                response.BodyWriter.WriteToPipe(readResponse.Value);
            else
                await response.BodyWriter.WriteAsync(
                    new ReadOnlyMemory<byte>(JsonSerializer.SerializeToUtf8Bytes(new { })), cancellationToken);

            await response.BodyWriter.FlushAsync(cancellationToken);
            await response.BodyWriter.CompleteAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        private static void WriteToPipe(this PipeWriter writer, ReadOnlySpan<byte> value) =>
            writer.WriteToPipe(value, Encoding.UTF8.GetEncoder());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="encoder"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteToPipe(this PipeWriter writer, ReadOnlySpan<byte> value, Encoder encoder)
        {
            Span<char> charSpan = stackalloc char[value.Length];
            for (var counter = 0; counter < value.Length; counter++)
            {
                charSpan[counter] = (char) value[counter];
            }

            var bytesNeeded = encoder.GetByteCount(charSpan, true);
            var bytesWritten = encoder.GetBytes(charSpan, writer.GetSpan(bytesNeeded), true);

            // Advance the PipeWriter to indicate how much data was written.
            writer.Advance(bytesWritten);
        }
    }
}