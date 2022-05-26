namespace Unite.Composer.Identity.Services
{
    public interface ISessionService<TIdentity, TSession>
    {
        TSession FindSession(TIdentity identity, TSession session);
        TSession CreateSession(TIdentity identity, string client);
        void RemoveSession(TSession session);
    }
}
