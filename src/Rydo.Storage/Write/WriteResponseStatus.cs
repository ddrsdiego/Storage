namespace Rydo.Storage.Write
{
    using Microsoft.AspNetCore.Http;

    public enum WriteResponseStatus
    {
        Ok = StatusCodes.Status200OK,
        Created = StatusCodes.Status201Created,
        InternalServerError = StatusCodes.Status500InternalServerError
    }
}