using MTCGServer.BLL.Exceptions;
using MTCGServer.DAL.Exceptions;
using MTCGServer.DAL.Users;
using MTCGServer.Models;
using Npgsql;

namespace MTCGServer.BLL
{
    public class UserManager : IUserManager
    {
        private readonly IUserDao _userDao;

        public UserManager(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public User? LoginUser(Credentials credentials)
        {
            try
            {
                return _userDao.GetUserByCredentials(credentials.Username, credentials.Password);
            }
            catch (DataAccessFailedException)
            {
                throw;
            }
        }

        public bool RegisterUser(Credentials credentials)
        {
            bool inserted = false;
            try
            {
                var user = new User(credentials);
                inserted = _userDao.InsertUser(user);
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
            }
            return inserted;
        }

        public User? GetUserByAuthToken(string authToken)
        {
            try
            {
                return _userDao.GetUserByAuthToken(authToken);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }

        public UserData? GetUserData(string username)
        {
            try
            {
                return _userDao.GetUserData(username);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }
        public string? GetTokenOfUsername(string username)
        {
            try
            {
                return _userDao.GetToken(username);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }

        public bool ExistUser(string username)
        {
            try
            {
                return _userDao.ExistUser(username);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }

        public bool UpdateUserData(UserData userData, string username)
        {
            try
            {
                return _userDao.UpdateUserData(userData, username);
            }
            catch (DataAccessFailedException)
            {
                throw new DataAccessException();
            }
        }
    }
}
