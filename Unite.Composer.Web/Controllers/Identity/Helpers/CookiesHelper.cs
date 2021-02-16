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
                    SameSite = SameSiteMode.Lax,
                    Secure = true,
                    Expires = DateTime.Now.AddYears(1)
                });

            cookies.Append(
                CookieNames.TokenCookieName,
                token,
                new()
                {
                    SameSite = SameSiteMode.Lax,
                    Secure = true,
                    Expires = DateTime.Now.AddYears(1),
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
