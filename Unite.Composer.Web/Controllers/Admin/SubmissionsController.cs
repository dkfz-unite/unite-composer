using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Submissions;
using Unite.Composer.Admin.Submissions.Models;
using Unite.Composer.Web.Configuration.Constants;

namespace Unite.Composer.Web.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class SubmissionsController : Controller
{
    private readonly SubmissionService _submissionService;

    public SubmissionsController(SubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("")]
    public SubmissionTaskModel[] Get()
    {
        var submissionTasks = _submissionService.GetAll().ToArray();

        return submissionTasks;
    }
    
    [HttpPost("{id}")]
    public bool UpdateSubmissionToPrepared(string id)
    {
        var approveStatus = _submissionService.UpdateSubmissionToPrepared(id);
        return approveStatus;
    }

    [HttpPost("{id}/updateRejectComment/{comment}")]
    public bool UpdateRejectComment(string id, string comment)
    {
       var approveStatus = _submissionService.UpdateRejectReason(id, comment);
        return approveStatus;
    }
}