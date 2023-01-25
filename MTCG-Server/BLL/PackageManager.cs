using MTCGServer.BLL.Exceptions;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL.Packages;
using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.BLL
{
    public class PackageManager : IPackageManager
    {
        private readonly IPackageDao _packageDao;
        public PackageManager(IPackageDao packageDao)
        {
            _packageDao = packageDao;
        }
        public bool AddPackage(Package package)
        {
            try
            {
                return _packageDao.AddPackage(package);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw new DataAccessException();
                }
                if (ex is PostgresException)
                {
                    throw new DuplicateDataException();
                }
                else
                {
                    throw;
                }
            }
        }
        public List<Card>? BuyPackage(User user)
        {
            try
            {
                return _packageDao.BuyPackage(user);
            }
            catch (Exception ex)
            {
                if (ex is DataAccessFailedException)
                {
                    throw new DataAccessException();
                }
                else if (ex is NotEnoughMoneyException)
                {
                    throw new TooLessMoneyException();
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
