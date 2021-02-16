using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Models;
using Unite.Composer.Identity.Services;
using Unite.Composer.Validation;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Data.Entities.Identity;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    public class SignInController : Controller
    {
        private readonly IValidator<SignInModel> _validator;
        private readonly IValidationService _validationService;
        private readonly IIdentityService<User> _identityService;
        private readonly ISessionService<User, UserSession> _sessionService;
        private readonly ILogger _logger;


        public SignInController(
            IValidator<SignInModel> validator,
            IValidationService validationService,
            IIdentityService<User> identityService,
            ISessionService<User, UserSession> sessionService,
            ILogger<SignInController> logger)
        {
            _validator = validator;
            _validationService = validationService;
            _identityService = identityService;
            _sessionService = sessionService;
            _logger = logger;
        }


        public ActionResult Post([FromBody] SignInModel model)
        {
            if(!_validationService.ValidateParameter(model, _validator, out var modelErrorMessage))
            {
                _logger.LogWarning(modelErrorMessage);

                return BadRequest(modelErrorMessage);
            }

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
