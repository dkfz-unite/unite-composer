using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Configuration.Constants;

namespace Unite.Composer.Web.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class DataUserController : Controller
{
    private readonly DataUserService _dataUserService;
    
    public DataUserController(DataUserService dataUserService)
    {
        _dataUserService = dataUserService;
    }
    
    [HttpDelete("delete")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteDataUser(int id, [FromBody]int[] userIds)
    {
        await _dataUserService.DeleteDataUser(id, userIds);
        
        return Ok();
    }
}