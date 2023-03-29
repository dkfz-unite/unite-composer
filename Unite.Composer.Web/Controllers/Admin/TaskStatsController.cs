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
        var stats = _taskStatsService.GetTaskNumbersStats();

        return Json(stats);
    }

    [HttpGet("stats/submission")]
    public IActionResult GetSubmissionTasksStats()
    {
        var stats = _taskStatsService.GetTaskNumbersStats();

        return Json(stats);
    }

    [HttpGet("stats/annotation")]
    public IActionResult GetAnnotationTasksStats()
    {
        var stats = _taskStatsService.GetAnnotationTasksStats();

        return Json(stats);
    }

    [HttpGet("stats/indexing")]
    public IActionResult GetIndexingTasksStats()
    {
        var stats = _taskStatsService.GetIndexingTasksStats();

        return Json(stats);
    }
}
