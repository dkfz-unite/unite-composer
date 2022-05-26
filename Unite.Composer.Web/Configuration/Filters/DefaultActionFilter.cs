﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Unite.Composer.Web.Configuration.Filters
{
    public class DefaultActionFilter : IActionFilter
    {
        private readonly ILogger _logger;

        public DefaultActionFilter(ILogger<DefaultActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
