using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;

namespace Unite.Composer.Web.Controllers.Admin;

[Route("api/admin/[controller]")]
[ApiController]
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
        return Json(_taskStatsService.GetTaskNumbersStats());
    }

    [HttpGet("stats/{type}")]
    public IActionResult GetSpecificTasksStats(string type)
    {
        if (string.Equals(type, "submission", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetSubmissionTasksStats());
        else if (string.Equals(type, "annotation", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetAnnotationTasksStats());
        else if (string.Equals(type, "indexing", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetIndexingTasksStats());
        else
            return NotFound();
    }
}
