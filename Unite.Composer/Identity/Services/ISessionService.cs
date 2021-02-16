namespace Unite.Composer.Identity.Services
{
    public interface ISessionService<TIdentity, TSession>
    {
        TSession CreateSession(TIdentity identity, string client);
        TSession GetSession(TSession session);
        void RemoveSession(TSession session);
    }
}
