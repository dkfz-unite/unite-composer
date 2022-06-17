﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Models.Admin;
using Unite.Composer.Web.Resources.Admin;

namespace Unite.Composer.Web.Controllers.Identity
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Root")]
    public class UserController : Controller
    {
        private readonly UserService _userService;


        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public IActionResult Check(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest($"Parameter '{nameof(email)}' should be set");
            }

            var emailNormalized = email.Trim().ToLower();

            var user = _userService.GetUser(user => user.Email == emailNormalized);

            var available = user == null;

            return Json(available); 
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = _userService.GetUser(id);

            return user != null
                ? Json(new UserResource(user))
                : NotFound();
        }

        [HttpPost("")]
        public IActionResult Post([FromBody] AddUserModel model)
        {
            var user = _userService.Add(model.Email, model.Permissions);

            return user != null
                ? Json(new UserResource(user))
                : BadRequest($"User with email '{model.Email}' already exists");
        }

        [HttpPut("")]
        public IActionResult Put([FromBody] EditUserModel model)
        {
            var user = _userService.Update(model.Id.Value, model.Permissions);

            return user != null
                ? Json(new UserResource(user))
                : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.Delete(id);

            return deleted
                ? Ok()
                : NotFound();
        }
    }
}
