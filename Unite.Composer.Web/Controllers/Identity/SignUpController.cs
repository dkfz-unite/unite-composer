using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Identity.Services;
using Unite.Composer.Web.Models.Identity;

namespace Unite.Composer.Web.Controllers.Identity;

[Route("api/identity/[controller]")]
public class SignUpController : Controller
{
    private readonly IdentityService _identityService;

    public SignUpController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] SignUpModel signUpModel)
    {
        var user = _identityService.SignUpUser(signUpModel.Email, signUpModel.Password);

        return user != null ? Ok() : BadRequest($"Email address '{signUpModel.Email}' is not in access list or already registered");
    }
}
