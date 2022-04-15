using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Composer.Web.Models.Identity;
using Unite.Identity.Entities;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    public class SignInController : Controller
    {
        private readonly IIdentityService<User> _identityService;
        private readonly ISessionService<User, UserSession> _sessionService;
        private readonly ILogger _logger;


        public SignInController(
            IIdentityService<User> identityService,
            ISessionService<User, UserSession> sessionService,
            ILogger<SignInController> logger)
        {
            _identityService = identityService;
            _sessionService = sessionService;
            _logger = logger;
        }


        public IActionResult Post([FromBody] SignInModel model)
        {
            var user = _identityService.SignInUser(model.Email, model.Password);

            if (user == null)
            {
                var invalidCredentialsErrorMessage = $"Invalid login or password";

                _logger.LogWarning($"{invalidCredentialsErrorMessage} for user '{model.Email}'");

                return BadRequest(invalidCredentialsErrorMessage);
            }

            var client = Request.Headers["User-Agent"].ToString();

            var session = _sessionService.CreateSession(user, client);

            CookiesHelper.AddAuthorizationCookies(Response.Cookies, session.Session, session.Token);

            return Ok();
        }
    }
}
