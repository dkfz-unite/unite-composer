using System;
using Microsoft.AspNetCore.Mvc;

namespace Unite.Composer.Web.Controllers
{
    [Route("api/")]
    public class DefaultController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var date = DateTime.Now;

            return Json(date);
        }
    }
}
