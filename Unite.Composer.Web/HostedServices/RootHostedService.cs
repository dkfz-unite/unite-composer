using Unite.Composer.Admin.Constants;
using Unite.Composer.Admin.Services;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Configuration.Options;

namespace Unite.Composer.Web.HostedServices;

public class RootHostedService : BackgroundService
{
    private readonly RootOptions _options;
    private readonly UserService _userService;
    private readonly IdentityService _identityService;
    private readonly ILogger _logger;


    public RootHostedService(
        RootOptions options,
        UserService userService,
        IdentityService identityService,
        ILogger<RootHostedService> logger)
    {
        _options = options;
        _userService = userService;
        _identityService = identityService;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Delay 5 seconds to let the web api start working
        await Task.Delay(5000, cancellationToken);

        var user = _identityService.GetUser(_options.User);

        if (user == null)
        {
            _logger.LogInformation("Configuring 'Root' user");

            _userService.Add(_options.User, Permissions.RootPermissions);
            _identityService.SignUpUser(_options.User, _options.Password, true);
        }
    }
}
