namespace MTCGServer.Core.Routing
{
    public interface ICommand
    {
        Response.Response Execute();
    }
}
