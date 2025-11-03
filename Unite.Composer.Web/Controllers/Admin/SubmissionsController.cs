using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Configuration.Constants;
using Unite.Composer.Web.Models;

namespace Unite.Composer.Web.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class SubmissionsController : Controller
{
    private readonly SubmissionsService _submissionsService;

    public SubmissionsController(SubmissionsService submissionsService)
    {
        _submissionsService = submissionsService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var tasks = await _submissionsService.GetPedning();

        return Ok(tasks);
    }

    [HttpPost("approve")]
    public async Task<IActionResult> ApproveMany([FromBody] long[] ids = null)
    {
        var tatks = await _submissionsService.GetPedning(ids);

        foreach (var task in tatks)
        {
            await _submissionsService.Approve(task.Id);
        }

        return Ok();
    }
    
    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(long id)
    {
        var status = await _submissionsService.Approve(id);
        
        return status ? Ok() : NotFound();
    }

    [HttpPost("reject")]
    public async Task<IActionResult> RejectMany([FromBody] RejectSubmissionsModel model)
    {
        var tasks = await _submissionsService.GetPedning(model.Ids);

        foreach (var task in tasks)
        {
            await _submissionsService.Reject(task.Id, model.Reason);
        }

        return Ok();
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(long id, [FromBody] RejectSubmissionModel model)
    {
        var status = await _submissionsService.Reject(id, model.Reason);

        return status ? Ok() : NotFound();
    }
}
