namespace MTCGServer.Core.Routing
{
    public interface IRouter
    {
        ICommand? Resolve(Request.Request request);
    }
}