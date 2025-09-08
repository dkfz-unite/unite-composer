using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Admin.Services;
using Unite.Composer.Download.Services.Tsv;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Specimens;
using Unite.Indices.Entities.Basic.Specimens.Constants;
using Unite.Indices.Entities.Specimens;
using Unite.Indices.Search.Engine.Queries;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Specimens.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

namespace Unite.Composer.Web.Controllers.Domain.Specimens;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SpecimensController : DomainController
{
    private readonly ISearchService<SpecimenIndex> _searchService;
    private readonly SpecimensDownloadService _tsvDownloadService;
    private readonly TaskStatsService _taskStatsService;


    public SpecimensController(
        ISearchService<SpecimenIndex> searchService, 
        SpecimensDownloadService tsvDownloadService,
        TaskStatsService taskStatsService)
    {
        _searchService = searchService;
        _tsvDownloadService = tsvDownloadService;
        _taskStatsService = taskStatsService;
    }


    [HttpPost("{type}")]
    public async Task<IActionResult> Search(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        Reassign(ref criteria, type);

        var result = await _searchService.Search(criteria);

        return Ok(From(result));
    }

    [HttpPost("{type}/stats")]
    public async Task<IActionResult> Stats(string type, [FromBody]SearchCriteria searchCriteria)
    {
        var criteria = searchCriteria ?? new SearchCriteria();
        Reassign(ref criteria, type);

        var stats = await _searchService.Stats(criteria);

        return Ok(new SpecimenDataResource(stats, type));
    }

    [HttpPost("{type}/data")]
    public async Task<ActionResult> Data(string type, [FromBody]BulkDownloadModel model)
    {
        Response.Headers.Append("Content-Disposition", "attachment; filename=\"data.zip\"");
        Response.ContentType = "application/zip";

        var criteria = model.Criteria ?? new SearchCriteria();
        Reassign(ref criteria, type);

         var stats = await _searchService.Stats(criteria);
        var originalIds = stats.Keys.Cast<int>().ToArray();

        var stream = Response.BodyWriter.AsStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        await _tsvDownloadService.Download(originalIds, model.Data, archive);
        
        return new EmptyResult();
    }

    [HttpGet("{type}/status")]
    public async Task<IActionResult> Status(string type)
    {
        var status = await _taskStatsService.GetStatus(Unite.Data.Entities.Tasks.Enums.IndexingTaskType.Specimen);

        return Ok(status);
    }


    private static SearchResult<SpecimenResource> From(SearchResult<SpecimenIndex> searchResult)
    {
        return new SearchResult<SpecimenResource>()
        {
            Total = searchResult.Total,
            Rows = searchResult.Rows.Select(index => new SpecimenResource(index)).ToArray()
        };
    }

    private static void Reassign(ref SearchCriteria searchCriteria, string type)
    {
        // TODO: Find a better way to use data filters without reassignment

        var comparison = StringComparison.InvariantCultureIgnoreCase;

        if (type.Equals(SpecimenType.Material, comparison))
            AssignFrom(ref searchCriteria, searchCriteria.Material, type);
        else if (type.Equals(SpecimenType.Line, comparison))
            AssignFrom(ref searchCriteria, searchCriteria.Line, type);
        else if (type.Equals(SpecimenType.Organoid, comparison))
            AssignFrom(ref searchCriteria, searchCriteria.Organoid, type);
        else if (type.Equals(SpecimenType.Xenograft, comparison))
            AssignFrom(ref searchCriteria, searchCriteria.Xenograft, type);
        else
            AssignFrom(ref searchCriteria, null, type);
    }

    private static void AssignFrom(ref SearchCriteria searchCriteria, in SpecimenCriteria specimenCriteria, in string specimenType)
    {
        searchCriteria.Specimen = (searchCriteria.Specimen ?? new SpecimenCriteria()) with
        {
            SpecimenType = new ValuesCriteria<string>([specimenType]),
            HasExp = specimenCriteria?.HasExp,
            HasExpSc = specimenCriteria?.HasExpSc,
            HasSms = specimenCriteria?.HasSms,
            HasCnvs = specimenCriteria?.HasCnvs,
            HasSvs = specimenCriteria?.HasSvs,
            HasMeth = specimenCriteria?.HasMeth
        };
    }
}
