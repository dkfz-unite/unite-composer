using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Lolliplot.Data;

namespace Unite.Composer.Web.Controllers.Visualization;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LolliplotController : Controller
{
    private readonly ProteinPlotDataService _dataService;

    public LolliplotController(ProteinPlotDataService dataService)
    {
        _dataService = dataService;
    }

    [HttpGet("transcript/{id}")]
    public async Task<ProteinPlotData> Get(long id)
    {
        return await _dataService.LoadData(id);
    }

    [HttpGet("gene/{id}/transcripts")]
    public async Task<Transcript[]> GetGeneTranscripts(int id)
    {
        return await _dataService.GetGeneTranscripts(id);
    }

    [HttpGet("mutation/{id}/transcripts")]
    public async Task<Transcript[]> GetMutationTranscripts(string id)
    {
        return await _dataService.GetMutationTranscripts(id);
    }
}
