using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Submissions;

namespace Unite.Composer.Web.Controllers.Data.Submissions;

[Route("api/data/[controller]")]
[ApiController]
public class SubmissionsController : Controller
{
    private readonly SubmissionService _submissionService;

    public SubmissionsController(SubmissionService submissionService)
    {
        _submissionService = submissionService;
    }

    [HttpGet("{id}")]
    public Unite.Data.Entities.Tasks.Task FindTaskStatus(string id)
    {
       var task = _submissionService.FindTaskStatus(id);
        return task;
    }
}