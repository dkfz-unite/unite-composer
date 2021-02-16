namespace Unite.Composer.Identity.Services
{
    public interface IAccessibilityService
    {
        bool IsConfigured();
        bool IsAllowed(string email);
    }
}
