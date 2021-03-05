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
                CookieNames.SessionCookie,
                session,
                new()
                {
                    Expires = DateTime.Now.AddMonths(1)
                }
            );

            cookies.Append(
                CookieNames.TokenCookie,
                token,
                new()
                {
                    Expires = DateTime.Now.AddMonths(1),
                    HttpOnly = true
                });
        }

        public static void RemoveAuthorizationCookies(IResponseCookies cookies)
        {
            cookies.Delete(CookieNames.SessionCookie);
            cookies.Delete(CookieNames.TokenCookie);
        }

        public static (string Session, string Token)? GetAuthorizationCookies(IRequestCookieCollection cookies)
        {
            if (cookies.TryGetValue(CookieNames.SessionCookie, out var session) &&
               cookies.TryGetValue(CookieNames.TokenCookie, out var token))
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
