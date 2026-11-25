using Microsoft.AspNetCore.Http;

namespace Unite.Composer.Clients.DonorFeed;

public class DonorFeedApiClient
{
    private readonly IDonorFeedOptions _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DonorFeedApiClient(IDonorFeedOptions options, IHttpContextAccessor httpContextAccessor)
    {
        _options = options;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task IndexProject(int id)
    {
        using var httpClient = new JsonHttpClient(_options.Host);

        var url = $"/api/indexing/projects/{id}";

        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
        
        await httpClient.PostAsync(url, ("Authorization", authHeader));
    }
}