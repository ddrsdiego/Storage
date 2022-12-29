namespace Rydo.Storage.Middlewares
{
    using System.Threading.Tasks;

    public interface IMessageMiddleware
    {
        /// <summary>
        /// The method that is called when the middleware is invoked
        /// </summary>
        /// <param name="context">The message context</param>
        /// <param name="next">A delegate to the next middleware</param>
        /// <returns></returns>
        Task Invoke(MessageConsumerContext context, MiddlewareDelegate next);
    }
}