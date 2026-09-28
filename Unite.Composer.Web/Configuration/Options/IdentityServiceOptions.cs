using Unite.Composer.Clients.Identity;

namespace Unite.Composer.Web.Configuration.Options;

public class IdentityServiceOptions : IIdentityServiceOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_IDENTITY_HOST");
}