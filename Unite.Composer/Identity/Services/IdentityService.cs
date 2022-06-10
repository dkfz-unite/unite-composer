﻿using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Entities;
using Unite.Identity.Services;

namespace Unite.Composer.Identity.Services
{
    public class IdentityService
    {
        private readonly IdentityDbContext _dbContext;

        public IdentityService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User GetUser(string email)
        {
            var user = _dbContext.Set<User>()
                .Include(user => user.UserSessions)
                .Include(user => user.UserPermissions)
                .FirstOrDefault(user => user.Email == email);

            return user;
        }

        public User SignUpUser(string email, string password, bool isRoot = false)
        {
            var passwordHash = GetStringHash(password);

            var user = _dbContext.Set<User>().FirstOrDefault(user =>
                user.Email == email &&
                user.IsRegistered == false
            );

            if (user != null)
            {
                user.Password = passwordHash;
                user.IsRoot = isRoot;

                _dbContext.Update(user);
                _dbContext.SaveChanges();

                return user;
            }

            return user;
        }

        public User SignInUser(string email, string password)
        {
            var passwordHash = GetStringHash(password);

            var user = _dbContext.Set<User>()
                .Include(user => user.UserPermissions)
                .FirstOrDefault(user =>
                    user.Email == email &&
                    user.Password == passwordHash
                );

            return user;
        }

        public User ChangePassword(string email, string oldPassword, string newPassword)
        {
            var passwordHash = GetStringHash(newPassword);

            var user = SignInUser(email, oldPassword);

            if (user != null)
            {
                user.Password = passwordHash;

                _dbContext.Update(user);
                _dbContext.SaveChanges();
            }

            return user;
        }

        private static string GetStringHash(string value)
        {
            var md5 = new MD5CryptoServiceProvider();

            var bytes = Encoding.ASCII.GetBytes(value);
            var hash = md5.ComputeHash(bytes);
            var hashString = Encoding.ASCII.GetString(hash);

            return hashString;
        }
    }
}
