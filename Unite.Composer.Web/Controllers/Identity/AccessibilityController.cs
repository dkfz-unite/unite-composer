using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Unite.Composer.Identity.Services;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/[controller]")]
    public class AccessibilityController : Controller
    {
        private readonly IAccessibilityService _accessibilityService;
        private readonly ILogger _logger;

        public AccessibilityController(
            IAccessibilityService accessibilityService,
            ILogger<AccessibilityController> logger)
        {
            _accessibilityService = accessibilityService;
            _logger = logger;
        }


        [HttpGet]
        public ActionResult Get(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                var modelErrorMessage = $"Request parameter {nameof(email)} is missing";

                _logger.LogWarning(modelErrorMessage);

                return BadRequest(modelErrorMessage);
            }

            if (!_accessibilityService.IsConfigured())
            {
                var notConfiguredErrorMessage = "Access list is not configured";

                _logger.LogCritical(notConfiguredErrorMessage);

                return NotFound(notConfiguredErrorMessage);
            }

            var accessAllowed = _accessibilityService.IsAllowed(email);

            if (!accessAllowed)
            {
                var notInAccessListErrorMessage = $"Email '{email}' is not in access list.";

                _logger.LogWarning(notInAccessListErrorMessage);

                return NotFound(notInAccessListErrorMessage);
            }

            return Ok();
        }
    }
}
