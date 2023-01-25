using MTCGServer.Models;

namespace MTCGServer.DAL.Users
{
    public class MemoryUserDao : IUserDao
    {
        private readonly List<User> _users = new();

        public bool ExistUser(string username)
        {
            User? user = _users.SingleOrDefault(u => u.Credentials.Username == username);
            if (user == null)
            {
                return false;
            }
            return true;
        }

        public User? GetUserByAuthToken(string authToken)
        {
            return _users.SingleOrDefault(u => u.Token == authToken);
        }

        public User? GetUserByCredentials(string username, string password)
        {
            return _users.SingleOrDefault(u => u.Credentials.Username == username && u.Credentials.Password == password);
        }

        public bool RegisterUser(User user)
        {
            Console.WriteLine("bin im MemoryUserDao");
            var inserted = false;

            if (GetUserByUsername(user.Credentials.Username) == null)
            {
                _users.Add(user);
                inserted = true;
            }

            return inserted;
        }

        public bool UpdateUserData(UserData userData, string username)
        {
            User? user = _users.SingleOrDefault(u => u.Credentials.Username.Equals(username));
            if (user != null)
            {
                user.UserData = userData;
                return true;
            }

            return false;
        }

        string? IUserDao.GetToken(string username)
        {
            string? token = null;
            User? user = _users.SingleOrDefault(u => u.Credentials.Username.Equals(username));
            if (user != null)
            {
                token = user.Token;

            }

            return token;
        }

        User? IUserDao.GetUserByAuthToken(string authToken)
        {
            return _users.SingleOrDefault(u => u.Token == authToken);
        }

        User? IUserDao.GetUserByCredentials(string username, string password)
        {
            return _users.SingleOrDefault(u => u.Credentials.Username == username && u.Credentials.Password == password);
        }

        private User? GetUserByUsername(string username)
        {
            return _users.SingleOrDefault(u => u.Credentials.Username == username);
        }

        UserData? IUserDao.GetUserData(string username)
        {
            User? user = _users.SingleOrDefault(u => u.Credentials.Username == username);
            return user != null ? user.UserData : null;
        }

        bool IUserDao.InsertUser(User user)
        {
            _users.Add(user);
            return true;
        }
    }
}
