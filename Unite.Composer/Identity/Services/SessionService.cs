using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Entities;
using Unite.Identity.Services;

namespace Unite.Composer.Identity.Services
{
    public class SessionService : ISessionService<User, UserSession>
    {
        private readonly IdentityDbContext _dbContext;

        public SessionService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserSession CreateSession(User identity, string client)
        {
            var session = Guid.NewGuid().ToString();
            var token = Guid.NewGuid().ToString();

            var userSession = new UserSession()
            {
                UserId = identity.Id,
                Client = client,
                Session = session,
                Token = token
            };

            _dbContext.UserSessions.Add(userSession);
            _dbContext.SaveChanges();

            return userSession;
        }

        public UserSession GetSession(UserSession session)
        {
            var userSession = _dbContext.UserSessions
                .Include(
                    userSession => userSession.User
                )
                .FirstOrDefault(userSession =>
                    userSession.Session == session.Session &&
                    userSession.Token == session.Token
                );

            return userSession;
        }

        public void RemoveSession(UserSession session)
        {
            _dbContext.UserSessions.Remove(session);
            _dbContext.SaveChanges();
        }
    }
}
