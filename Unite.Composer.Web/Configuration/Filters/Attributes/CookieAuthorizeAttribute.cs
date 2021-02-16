using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Data.Entities.Identity;

namespace Unite.Composer.Web.Configuration.Filters.Attributes
{
    public class CookieAuthorizeAttribute : ActionFilterAttribute
    {
        public CookieAuthorizeAttribute()
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAuthorized = IsAuthorized(context.HttpContext);

            if (!isAuthorized)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                base.OnActionExecuting(context);
            }
        }

        private bool IsAuthorized(HttpContext context)
        {
            var sessionService = ResolveService<ISessionService<User, UserSession>>(context.RequestServices);

            var cookies = CookiesHelper.GetAuthorizationCookies(context.Request.Cookies);

            if (cookies == null)
            {
                return false;   
            }

            var session = sessionService.GetSession(new() { Session = cookies.Value.Session, Token = cookies.Value.Token });

            if(session == null)
            {
                return false;
            }

            return true;
        }

        private T ResolveService<T>(IServiceProvider services) where T : class
        {
            var serviceType = typeof(T);
            var serviceObject = services.GetService(serviceType);
            var service = serviceObject as T;

            if(service == null)
            {
                throw new Exception($"Could not resolve service of type '{serviceType.FullName}' from service collection");
            }

            return service;
        }
    }
}
