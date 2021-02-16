﻿using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Models;
using Unite.Composer.Identity.Resources;
using Unite.Composer.Identity.Services;
using Unite.Composer.Validation;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Data.Entities.Identity;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    public class AccountController : Controller
    {
        private readonly IValidator<PasswordChangeModel> _passwordChangeModelValidator;
        private readonly IValidationService _validationService;
        private readonly IIdentityService<User> _identityService;
        private readonly ISessionService<User, UserSession> _sessionService;
        private readonly ILogger _logger;


        public AccountController(
            IValidator<PasswordChangeModel> passwordChangeModelValidator,
            IValidationService validationService,
            IIdentityService<User> identityService,
            ISessionService<User, UserSession> sessionService,
            ILogger<AccountController> logger)
        {
            _passwordChangeModelValidator = passwordChangeModelValidator;
            _validationService = validationService;
            _identityService = identityService;
            _sessionService = sessionService;
            _logger = logger;
        }


        [HttpGet]
        [CookieAuthorize]
        public ActionResult Get()
        {
            var currentUser = GetCurrentUser(Request);

            var account = CreateFrom(currentUser);

            return Json(account);
        }

        [HttpPut]
        [CookieAuthorize]
        public ActionResult Put([FromBody] PasswordChangeModel model)
        {
            if (!_validationService.ValidateParameter(model, _passwordChangeModelValidator, out var modelErrorMessage))
            {
                _logger.LogWarning(modelErrorMessage);

                return BadRequest(modelErrorMessage);
            }

            var currentUser = GetCurrentUser(Request);

            var updatedUser = _identityService.ChangePassword(currentUser.Email, model.OldPassword, model.NewPassword);

            if(updatedUser == null)
            {
                var invalidPasswordMessage = $"Invalid old password";

                _logger.LogWarning($"{invalidPasswordMessage} for user '{currentUser.Email}'");

                return BadRequest(invalidPasswordMessage);
            }

            var account = CreateFrom(currentUser);

            return Json(account);
        }


        private User GetCurrentUser(HttpRequest request)
        {
            // Should neven be null since CookieAuthorize filter was passed
            var cookies = CookiesHelper.GetAuthorizationCookies(request.Cookies);

            // Should neven be null since CookieAuthorize filter was passed
            var session = _sessionService.GetSession(new() { Session = cookies.Value.Session, Token = cookies.Value.Token });

            // Should neven be null since CookieAuthorize filter was passed
            var user = _identityService.FindUser(session.User.Email);

            return user;
        }

        private Account CreateFrom(User user)
        {
            var account = new Account();

            account.Email = user.Email;

            account.Devices = user.UserSessions?
                .Select(session => session.Client)
                .ToArray();

            return account;
        }
    }
}
