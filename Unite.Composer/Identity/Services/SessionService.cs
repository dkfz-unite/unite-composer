using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Identity;
using Unite.Data.Services;

namespace Unite.Composer.Identity.Services
{
    public class SessionService : ISessionService<User, UserSession>
    {
        private readonly UniteDbContext _database;

        public SessionService(UniteDbContext database)
        {
            _database = database;
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

            _database.UserSessions.Add(userSession);
            _database.SaveChanges();

            return userSession;
        }

        public UserSession GetSession(UserSession session)
        {
            var userSession = _database.UserSessions
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
            _database.UserSessions.Remove(session);
            _database.SaveChanges();
        }
    }
}
