using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Models.Identity;
using Unite.Composer.Web.Services.Validation;
using Unite.Data.Entities.Identity;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/identity/[controller]")]
    public class SignUpController : Controller
    {
        private readonly IValidator<SignUpModel> _validator;
        private readonly IValidationService _validationService;
        private readonly IAccessibilityService _accessibilityService;
        private readonly IIdentityService<User> _identityService;
        private ILogger _logger;

        public SignUpController(
            IValidator<SignUpModel> validator,
            IValidationService validationService,
            IAccessibilityService accessibilityService,
            IIdentityService<User> identityService,
            ILogger<SignUpController> logger)
        {
            _validator = validator;
            _validationService = validationService;
            _accessibilityService = accessibilityService;
            _identityService = identityService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Post([FromBody] SignUpModel signUpModel)
        {
            if (!_validationService.ValidateParameter(signUpModel, _validator, out var modelErrorMessage))
            {
                _logger.LogWarning(modelErrorMessage);

                return BadRequest(modelErrorMessage);
            }

            if (!_accessibilityService.IsConfigured())
            {
                var notConfiguredErrorMessage = "Access list is not configured";

                _logger.LogCritical(notConfiguredErrorMessage);

                return NotFound(notConfiguredErrorMessage);
            }

            if (!_accessibilityService.IsAllowed(signUpModel.Email))
            {
                var notInListErrorMessage = $"Email address '{signUpModel.Email}' is not in access list";

                _logger.LogWarning(notInListErrorMessage);

                return NotFound(notInListErrorMessage);
            }

            var user = _identityService.SignUpUser(signUpModel.Email, signUpModel.Password);

            if (user == null)
            {
                var userExistsErrorMessage = $"User with email '{signUpModel.Email}' already exists";

                _logger.LogWarning(userExistsErrorMessage);

                return BadRequest(userExistsErrorMessage);
            }

            return Ok();
        }
    }
}
