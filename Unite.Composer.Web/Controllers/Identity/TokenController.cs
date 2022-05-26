using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Identity.Entities;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    public class TokenController : Controller
    {
        private readonly ApiOptions _apiOptions;
        private readonly IIdentityService<User> _identityService;
        private readonly ISessionService<User, UserSession> _sessionService;
        private readonly ILogger _logger;


        public TokenController(
            ApiOptions apiOptions,
            IIdentityService<User> identityService,
            ISessionService<User, UserSession> sessionService,
            ILogger<TokenController> logger)
        {
            _apiOptions = apiOptions;
            _identityService = identityService;
            _sessionService = sessionService;
            _logger = logger;
        }


        [HttpPost]
        public IActionResult Post(string login)
        {
            var session = CookieHelper.GetSessionCookie(Request);

            if (session == null)
            {
                _logger.LogWarning("Invalid attempt to refresh authorization token");

                return Unauthorized();
            }

            var user = _identityService.FindUser(login);

            if (user == null)
            {
                _logger.LogWarning("Invalid attempt to refresh authorization token for not existing user");

                return BadRequest();
            }

            var userSession = _sessionService.FindSession(user, new() { Session = session });

            if (userSession == null)
            {
                _logger.LogWarning("Invalid attempt to refresh authorization token for not existing session");

                return BadRequest();
            }

            var identity = ClaimsHelper.GetIdentity(user);

            var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key);

            CookieHelper.SetSessionCookie(Response, userSession.Session);

            return Ok(token);
        }
    }
}
