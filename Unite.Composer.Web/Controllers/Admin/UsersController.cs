using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.Controllers.Identity.Helpers;
using Unite.Composer.Web.Resources.Admin;

namespace Unite.Composer.Web.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = "Root")]
public class UsersController : Controller
{
    private readonly UserService _userService;
    private readonly RootOptions _rootOptions;


    public UsersController(
        UserService userService,
        RootOptions rootOptions)
    {
        _userService = userService;
        _rootOptions = rootOptions;
    }

    [HttpGet("")]
    public IActionResult GetAll()
    {
        var currentUserEmail = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);
        var rootUserEmail = _rootOptions.User;

        var users = _userService
            .GetUsers(user => user.Email != currentUserEmail && user.Email != rootUserEmail)
            .Select(user => new UserResource(user))
            .ToArray();

        return Json(users);
    }
}
