using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Web.Configuration.Constants;

namespace Unite.Composer.Web.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class TasksController : Controller
{
    private readonly TaskStatsService _taskStatsService;


    public TasksController(TaskStatsService taskStatsService)
    {
        _taskStatsService = taskStatsService;
    }

    [HttpGet("stats")]
    public IActionResult GetGeneralTasksStats()
    {
        return Json(_taskStatsService.GetTaskNumbersStats().Result);
    }

    [HttpGet("stats/{type}")]
    public IActionResult GetSpecificTasksStats(string type)
    {
        if (string.Equals(type, "submission", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetSubmissionTasksStats().Result);
        else if (string.Equals(type, "annotation", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetAnnotationTasksStats().Result);
        else if (string.Equals(type, "indexing", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetIndexingTasksStats().Result);
        else
            return NotFound();
    }
}
