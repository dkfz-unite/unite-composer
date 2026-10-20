using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data;
using Unite.Composer.Data.Projects;
using Unite.Composer.Download.Services.Tsv;
using Unite.Composer.Web.Configuration.Constants;
using Unite.Composer.Web.Extensions;
using Unite.Composer.Web.Models;
using Unite.Composer.Web.Resources.Domain.Projects;
using Unite.Data.Context;
using Unite.Data.Entities.Donors;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using ProjectIndex = Unite.Indices.Entities.Projects.ProjectIndex;

namespace Unite.Composer.Web.Controllers.Domain.Projects;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectController : DomainController
{
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    private readonly ISearchService<ProjectIndex> _projectSearchService;
    private readonly ISearchService<DonorIndex> _donorsSearchService;
    private readonly DonorsDownloadService _tsvDownloadService;
    private readonly ProjectService _projectService;

    public record UpdateModel(string Description);


    public ProjectController(
        IDbContextFactory<DomainDbContext> dbContextFactory,
        ISearchService<ProjectIndex> projectsSearchService,
        ISearchService<DonorIndex> donorsSearchService,
        DonorsDownloadService tsvDownloadService, ProjectService projectService)
    {
        _dbContextFactory = dbContextFactory;
        _projectSearchService = projectsSearchService;
        _donorsSearchService = donorsSearchService;
        _tsvDownloadService = tsvDownloadService;
        _projectService = projectService;
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Project(int id)
    {
        var key = id.ToString();

        var result = await _projectSearchService.Get(new PersonalGetCriteria(key, new UserClaims(User.GetUserId(), User.GetIsRoot())));

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

        var project = await dbContext.Set<Project>()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id);

        if (project == null)
            return NotFound();

        // Only donors of the project, which are accessible to the user
        var criteria = new SearchCriteria { Donor = new DonorCriteria { Project = new ValuesCriteria<string>([project.Name]) } };
        var stats = await _donorsSearchService.Stats(new PersonalSearchCriteria(User.GetUserId(), User.GetIsRoot(), criteria));
        var ids = stats.Keys.Cast<int>().ToArray();

        if (ids.Length == 0)
            return NotFound();

        Response.ContentType = "application/octet-stream";
        Response.Headers.Append("Content-Disposition", "attachment; filename=data.zip");

        using var stream = Response.BodyWriter.AsStream();
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true);
        await _tsvDownloadService.Download(ids, model.Data, archive);

        return new EmptyResult();
    }
    
    [HttpPost("{id}/users")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> AssignUser(int id, [FromBody]int[] userIds)
    {
        var projectUser = await _projectService.AssignUserToProject(userIds, id);
        if (projectUser == null)
            return NotFound();

        return Ok(projectUser);
    }
    
    [HttpDelete("{id}/users")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> RemoveUser(int id, [FromBody]int[] userIds)
    {
        await _projectService.RemoveUserFromProject(userIds, id);

        return Ok();
    }
    
    [HttpGet("{id}/users")]
    public async Task<IActionResult> GetProjectUsers(int id)
    {
        var projectUsers = await _projectService.ListProjectUsers(id);

        return Ok(projectUsers);
    }
    
    [HttpPost("{id}/is-public")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> SetIsPublic(int id, [FromBody]bool isPublic)
    {
        await _projectService.SetIsPublic(id, isPublic);
        
        return Ok();
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
