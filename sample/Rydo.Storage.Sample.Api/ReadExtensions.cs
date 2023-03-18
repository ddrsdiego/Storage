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
        public static async Task WriteResponseAsync(this ValueTask<ReadResponse> taskReadResponse,
            HttpResponse response, CancellationToken cancellationToken = default)
        {
            var readResponse = await taskReadResponse;
            await readResponse.WriteResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskReadResponse"></param>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        public static async Task WriteResponseAsync(this Task<ReadResponse> taskReadResponse, HttpResponse response,
            CancellationToken cancellationToken = default)
        {
            var readResponse = await taskReadResponse;
            await readResponse.WriteResponseAsync(response, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readResponse"></param>
        /// <param name="response"></param>
        /// <param name="cancellationToken"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task WriteResponseAsync(this ReadResponse readResponse, HttpResponse response,
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

            await response.BodyWriter.CompleteAsync();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pipe"></param>
        private static void WriteToPipe(this IReadOnlyList<byte> value, PipeWriter pipe) =>
            value.WriteToPipe(pipe, Encoding.UTF8.GetEncoder());
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pipe"></param>
        /// <param name="encoder"></param>
        private static void WriteToPipe(this IReadOnlyList<byte> value, PipeWriter pipe, Encoder encoder) =>
            pipe.WriteToPipe(value, encoder);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="value"></param>
        public static void WriteToPipe(this PipeWriter pipe, IReadOnlyList<byte> value) =>
            pipe.WriteToPipe(value, Encoding.UTF8.GetEncoder());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pipe"></param>
        /// <param name="value"></param>
        /// <param name="encoder"></param>
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