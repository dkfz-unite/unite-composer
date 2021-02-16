using System;
using Microsoft.AspNetCore.Http;
using Unite.Composer.Identity.Constants;

namespace Unite.Composer.Web.Controllers.Identity.Helpers
{
    internal static class CookiesHelper
    {
        public static void AddAuthorizationCookies(IResponseCookies cookies, string session, string token)
        {
            cookies.Append(
                CookieNames.SessionCookieName,
                session,
                new()
                {
                    Expires = DateTime.Now.AddMonths(11)
                });

            cookies.Append(
                CookieNames.TokenCookieName,
                token,
                new()
                {
                    SameSite = SameSiteMode.Strict,
                    Secure = true,
                    Expires = DateTime.Now.AddMonths(11),
                    HttpOnly = true
                });
        }

        public static void RemoveAuthorizationCookies(IResponseCookies cookies)
        {
            cookies.Delete(CookieNames.SessionCookieName);
            cookies.Delete(CookieNames.TokenCookieName);
        }

        public static (string Session, string Token)? GetAuthorizationCookies(IRequestCookieCollection cookies)
        {
            if (cookies.TryGetValue(CookieNames.SessionCookieName, out var session) &&
               cookies.TryGetValue(CookieNames.TokenCookieName, out var token))
            {
                return (session, token);
            }
            else
            {
                return null;
            }
        }
    }
}
