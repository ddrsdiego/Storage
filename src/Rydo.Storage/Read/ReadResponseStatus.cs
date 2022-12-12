namespace Rydo.Storage.Read
{
    using Microsoft.AspNetCore.Http;

    public enum ReadResponseStatus
    {
        Ok = StatusCodes.Status200OK,
        NotFound = StatusCodes.Status404NotFound,
        InternalServerError = StatusCodes.Status500InternalServerError
    }
}