using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Analysis.Expression;
using Unite.Composer.Web.Services;
using Unite.Data.Entities.Tasks.Enums;

namespace Unite.Composer.Web.Controllers.Analysis;

[Route("api/analysis")]
[Authorize]
public class AnalysisController : Controller
{
    private readonly AnalysisTaskService _analysisTaskService;
    private readonly ExpressionAnalysisService _expressionAnalysisService;

    public AnalysisController(
        AnalysisTaskService analysisTaskService,
        ExpressionAnalysisService expressionAnalysisService)
    {
        _analysisTaskService = analysisTaskService;
        _expressionAnalysisService = expressionAnalysisService;
    }

    [HttpPost("dexp")]
    public IActionResult CreateDExpTask([FromBody] Composer.Analysis.Expression.Models.Analysis model)
    {
        model.Key = Guid.NewGuid().ToString();

        _analysisTaskService.Create(model.Key, model, AnalysisTaskType.DExp);

        return Ok(model.Key);
    }

    [HttpGet("{key}")]
    public IActionResult GetTaskStatus(string key)
    {
        var task = _analysisTaskService.Get(key);

        if (task == null)
            return NotFound();

        return Ok(task.StatusTypeId);
    }

    [HttpGet("{key}/results")]
    public async Task<IActionResult> GetTaskResults(string key)
    {
        var task = _analysisTaskService.Get(key);

        if (task == null)
            return NotFound();

        if (task.AnalysisTypeId == AnalysisTaskType.DExp)
            return Ok(await _expressionAnalysisService.LoadResult(key));
        
        return BadRequest("Task analysis type is not supported");
    }

    [HttpGet("{key}/data")]
    public async Task<IActionResult> GetTaskData(string key)
    {
        var task = _analysisTaskService.Get(key);

        if (task == null)
            return NotFound();

        if (task.AnalysisTypeId == AnalysisTaskType.DExp)
            return Ok(await _expressionAnalysisService.DownloadResult(key));

        return BadRequest("Task analysis type is not supported");
    }

    [HttpDelete("{key}")]
    public async Task<IActionResult> DeleteTask(string key)
    {
        var task = _analysisTaskService.Get(key);
        var statuses = new TaskStatusType?[] {TaskStatusType.Processed, TaskStatusType.Failed};

        if (task == null)
            return NotFound();

        if (!statuses.Contains(task.StatusTypeId))
            return BadRequest("Task can't be deleted");

        if (task.AnalysisTypeId == AnalysisTaskType.DExp)
            await _expressionAnalysisService.DeleteData(key);

        _analysisTaskService.Delete(task);

        return Ok();
    }
}
