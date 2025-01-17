using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Submissions;

namespace Unite.Composer.Web.Controllers.Data.Submissions;

[Route("api/data/[controller]")]
[ApiController]
[Authorize]
public class SubmissionController : Controller
{
    private readonly SubmissionsService _submissionsService;

    public SubmissionController(SubmissionsService submissionService)
    {
        _submissionsService = submissionService;
    }

    [HttpGet("{id}/status")]
    public async Task<SubmissionStatus> GetStatus(long id)
    {
        return await _submissionsService.GetStatus(id);
    }
}
