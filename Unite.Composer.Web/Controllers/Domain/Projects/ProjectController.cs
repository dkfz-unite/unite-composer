using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv;
using Unite.Composer.Web.Configuration.Constants;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Projects;
using Unite.Data.Context;
using Unite.Data.Entities.Donors;
using Unite.Indices.Search.Services;

using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;

namespace Unite.Composer.Web.Controllers.Domain.Projects;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : DomainController
{
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    private readonly ISearchService<ProjectIndex> _projectSearchService;
    private readonly DonorsTsvDownloadService _tsvDownloadService;

    public record UpdateModel(string Description);


    public ProjectController(
        IDbContextFactory<DomainDbContext> dbContextFactory,
        ISearchService<ProjectIndex> projectsSearchService,
        DonorsTsvDownloadService tsvDownloadService)
    {
        _dbContextFactory = dbContextFactory;
        _projectSearchService = projectsSearchService;
        _tsvDownloadService = tsvDownloadService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Project(int id)
    {
        var key = id.ToString();

        var result = await _projectSearchService.Get(key);

        return Ok(From(result));
    }

    [HttpGet("{id}/description")]
    public async Task<IActionResult> GetDescription(int id)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var project = await dbContext.Set<Project>()
            .AsNoTracking()
            .FirstOrDefaultAsync(project => project.Id == id);

        if (project == null)
            return NotFound();
        
        return Ok(project.Description);
    }

    [Authorize(Policy = Policies.Data.Writer)]
    [HttpPut("{id}/description")]
    public async Task<IActionResult> UpdateDescription(int id, [FromBody]UpdateModel model)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var project = await dbContext.Set<Project>()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id);

        if (project == null)
            return NotFound();

        project.Description = model.Description;

        dbContext.Update(project);
        await dbContext.SaveChangesAsync();

        return Ok(project.Description);
    }

    [HttpPost("{id}/data")]
    public async Task<IActionResult> Data(int id, [FromBody] SingleDownloadModel model)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var ids = dbContext.Set<ProjectDonor>()
            .AsNoTracking()
            .Where(entity => entity.ProjectId == id)
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArray();

        var bytes = await _tsvDownloadService.Download(ids, model.Data);

        return File(bytes, "application/zip", "data.zip");
    }


    private static ProjectResource From(ProjectIndex index)
    {
        if (index == null)
        {
            return null;
        }

        return new ProjectResource(index);
    }
}
