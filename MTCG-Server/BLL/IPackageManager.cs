using MTCGServer.Models;

namespace MTCGServer.BLL
{
    public interface IPackageManager
    {
        bool AddPackage(Package package);
        List<Card>? BuyPackage(User user);
    }
}
