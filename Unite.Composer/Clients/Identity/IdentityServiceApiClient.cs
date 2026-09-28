using Microsoft.AspNetCore.Http;

namespace Unite.Composer.Clients.Identity;

public class IdentityServiceApiClient
{
    private readonly IIdentityServiceOptions _options;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityServiceApiClient(IIdentityServiceOptions options, IHttpContextAccessor httpContextAccessor)
    {
        _options = options;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<UserResource[]> GetUsers()
    {
        using var httpClient = new JsonHttpClient(_options.Host);

        var url = $"/api/users";

        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
        
        return await httpClient.GetAsync<UserResource[]>(url, ("Authorization", authHeader));
    }
}