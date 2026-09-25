using Unite.Composer.Clients.DonorFeed;

namespace Unite.Composer.Web.Configuration.Options;

public class DonorFeedOptions : IDonorFeedOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_FEED_DONORS_HOST");
}