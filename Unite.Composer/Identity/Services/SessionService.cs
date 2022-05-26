using System;
using System.Linq;
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

            _dbContext.Add(userSession);
            _dbContext.SaveChanges();

            return userSession;
        }

        public UserSession FindSession(User identity, UserSession session)
        {
            var userSession = _dbContext
                .Set<UserSession>()
                .FirstOrDefault(userSession =>
                    userSession.UserId == identity.Id &&
                    userSession.Session == session.Session
                );

            return userSession;
        }

        public void RemoveSession(UserSession session)
        {
            _dbContext.Remove(session);
            _dbContext.SaveChanges();
        }
    }
}
