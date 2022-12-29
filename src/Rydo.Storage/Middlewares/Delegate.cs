namespace Rydo.Storage.Middlewares
{
    using System.Threading.Tasks;

    /// <summary>
    /// The delegate used to call the next middleware
    /// </summary>
    /// <param name="context">The message context to be passed to the next middleware</param>
    public delegate Task MiddlewareDelegate(MessageConsumerContext context);
}