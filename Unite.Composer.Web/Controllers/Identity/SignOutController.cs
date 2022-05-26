using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Identity.Entities;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    [Authorize]
    public class SignOutController : Controller
    {
        private readonly IIdentityService<User> _identityService;
        private readonly ISessionService<User, UserSession> _sessionService;
        private readonly ILogger _logger;

        public SignOutController(
            IIdentityService<User> identityService,
            ISessionService<User, UserSession> sessionService,
            ILogger<SignOutController> logger)
        {
            _identityService = identityService;
            _sessionService = sessionService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post()
        {
            //return Ok();

            // Refresh token functionality is not yet implemented

            var session = CookieHelper.GetSessionCookie(Request);

            if (session != null)
            {
                var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

                var user = _identityService.FindUser(email);

                if (user == null)
                {
                    _logger.LogWarning("Invalid attempt to sign out not existing user");

                    return BadRequest();
                }

                var userSession = _sessionService.FindSession(user, new() { Session = session });

                if (userSession == null)
                {
                    _logger.LogWarning("Invalid attempt to remove not existing session");

                    return BadRequest();
                }

                _sessionService.RemoveSession(userSession);

                CookieHelper.DeleteSessionCookie(Response);
            }

            return Ok();
        }
    }
}
