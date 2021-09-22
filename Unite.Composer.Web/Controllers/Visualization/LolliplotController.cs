using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Composer.Web.Configuration.Filters.Attributes;

namespace Unite.Composer.Web.Controllers.Visualization
{
    [Route("api/[controller]")]
    [ApiController]
    public class LolliplotController : Controller
    {
        private readonly ProteinPlotDataService _dataService;

        public LolliplotController(ProteinPlotDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("transcript/{id}")]
        [CookieAuthorize]
        public async Task<ProteinPlotData> Get(long id)
        {
            return await _dataService.LoadData(id);
        }

        [HttpGet("gene/{id}/transcripts")]
        [CookieAuthorize]
        public async Task<Transcript[]> GetGeneTranscripts(int id)
        {
            return await _dataService.GetGeneTranscripts(id);
        }

        [HttpGet("mutation/{id}/transcripts")]
        [CookieAuthorize]
        public async Task<Transcript[]> GetMutationTranscripts(long id)
        {
            return await _dataService.GetMutationTranscripts(id);
        }
    }
}
