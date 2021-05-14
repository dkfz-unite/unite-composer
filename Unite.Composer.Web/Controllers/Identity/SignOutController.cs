using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Data.Entities.Identity;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    public class SignOutController : Controller
    {
        private readonly ISessionService<User, UserSession> _sessionService;

        public SignOutController(
            ISessionService<User, UserSession> sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        [CookieAuthorize]
        public IActionResult Post()
        {
            var session = GetCurrentSession(Request);

            _sessionService.RemoveSession(session);

            CookiesHelper.RemoveAuthorizationCookies(Response.Cookies);

            return Ok();
        }

        private UserSession GetCurrentSession(HttpRequest request)
        {
            // Should neven be null since CookieAuthorize filter was passed
            var cookies = CookiesHelper.GetAuthorizationCookies(request.Cookies);

            // Should neven be null since CookieAuthorize filter was passed
            var session = _sessionService.GetSession(new() { Session = cookies.Value.Session, Token = cookies.Value.Token });

            return session;
        }
    }
}
