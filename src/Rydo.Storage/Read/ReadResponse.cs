namespace Rydo.Storage.Read
{
    using System;
    using System.Buffers;
    using System.Text.Json;

    public readonly struct ReadResponse
    {
        private ReadResponse(ReadResponseStatus statusCode, ReadRequest request, byte[]? value,
            Exception? exception = null)
        {
            StatusCode = statusCode;
            Value = value;
            Request = request;
            Exception = exception;
            RespondedAt = DateTime.Now;
            var timeSpan = RespondedAt - Request.RequestAt;

            ElapsedMilliseconds = timeSpan.Milliseconds;
        }

        public readonly ReadRequest Request;
        public readonly byte[]? Value;
        public readonly ReadResponseStatus StatusCode;
        public readonly Exception? Exception;
        public readonly DateTime RespondedAt;
        public readonly int ElapsedMilliseconds;

        public T GetRaw<T>()
        {
            var reader = new Utf8JsonReader(new ReadOnlySequence<byte>(Value));
            return JsonSerializer.Deserialize<T>(ref reader)!;
        }

        internal static ReadResponse GetResponseNotFound(ReadRequest request) =>
            new ReadResponse(ReadResponseStatus.NotFound, request, null);

        internal static ReadResponse GetResponseOk(ReadRequest request, byte[]? value) =>
            new ReadResponse(ReadResponseStatus.Ok, request, value);

        internal static ReadResponse GetResponseError(ReadRequest request, Exception exception) =>
            new ReadResponse(ReadResponseStatus.InternalServerError, request, null, exception);
    }
}