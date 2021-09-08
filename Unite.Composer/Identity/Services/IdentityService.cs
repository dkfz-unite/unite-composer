using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Entities;
using Unite.Identity.Services;

namespace Unite.Composer.Identity.Services
{
    public class IdentityService : IIdentityService<User>
    {
        private readonly IdentityDbContext _dbContext;

        public IdentityService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User FindUser(string login)
        {
            var loginNormalised = login.ToLower().Trim();

            var user = _dbContext.Users
                .Include(user => user.UserSessions)
                .FirstOrDefault(user => user.Email == loginNormalised);

            return user;
        }

        public User SignUpUser(string login, string password)
        {
            var loginNormalised = login.ToLower().Trim();
            var passwordHash = GetStringHash(password);

            var exists = _dbContext.Users.Any(user =>
                user.Email == loginNormalised
            );

            if (!exists)
            {
                var user = new User()
                {
                    Email = loginNormalised,
                    Password = passwordHash
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                return user;
            }
            else
            {
                return null;
            }
        }

        public User SignInUser(string login, string password)
        {
            var loginNormalised = login.ToLower().Trim();
            var passwordHash = GetStringHash(password);

            var user = _dbContext.Users.FirstOrDefault(user =>
                user.Email == loginNormalised &&
                user.Password == passwordHash
            );

            return user;
        }

        public User ChangePassword(string login, string oldPassword, string newPassword)
        {
            var user = SignInUser(login, oldPassword);

            if(user != null)
            {
                user.Password = GetStringHash(newPassword);

                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
            }

            return user;
        }

        private string GetStringHash(string value)
        {
            var md5 = new MD5CryptoServiceProvider();

            var bytes = Encoding.ASCII.GetBytes(value);
            var hash = md5.ComputeHash(bytes);
            var hashString = Encoding.ASCII.GetString(hash);

            return hashString;
        }
    }
}
