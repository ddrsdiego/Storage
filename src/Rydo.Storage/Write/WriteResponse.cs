namespace Rydo.Storage.Write
{
    using System;
    using Microsoft.AspNetCore.Http;

    public enum WriteResponseStatus
    {
        Created = StatusCodes.Status201Created,
        InternalServerError = StatusCodes.Status500InternalServerError
    }

    public readonly struct WriteResponse
    {
        private WriteResponse(WriteResponseStatus status, WriteRequest request, Exception exception)
        {
            Status = status;
            Exception = exception;
            Request = request;
            CreatedAt = DateTime.Now;
            ElapsedTimeInSeconds = CreatedAt.Subtract(request.RequestedAt).Seconds;
        }

        public readonly DateTime CreatedAt;
        public readonly WriteRequest Request;
        public readonly Exception Exception;
        public readonly int ElapsedTimeInSeconds;
        public readonly WriteResponseStatus Status;

        public static WriteResponse GetCreatedInstance(WriteRequest request) =>
            new WriteResponse(WriteResponseStatus.Created, request, null);

        public static WriteResponse GetErrorInstance(WriteRequest request, Exception exception) =>
            new WriteResponse(WriteResponseStatus.InternalServerError, request, exception);
    }
}