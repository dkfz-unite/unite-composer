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

    [HttpGet("stats/{type}")]
    public IActionResult GetTasksStats(string type)
    {
        if (string.Equals(type, "submission", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetTaskNumbersStats());
        else if (string.Equals(type, "annotation", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetAnnotationTasksStats());
        else if (string.Equals(type, "indexing", StringComparison.InvariantCultureIgnoreCase))
            return Json(_taskStatsService.GetIndexingTasksStats());
        else
            return Json(_taskStatsService.GetTaskNumbersStats());
    }
}
