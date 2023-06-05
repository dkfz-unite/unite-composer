using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Projects;
using Unite.Composer.Data.Projects.Models;
using Unite.Composer.Web.Configuration.Extensions;
using Unite.Composer.Web.Controllers.Data.Projects.Models;

namespace Unite.Composer.Web.Controllers.Data.Projects;

[Route("api/data/project")]
[ApiController]
[Authorize]
public class ProjectController : Controller
{
    private readonly ProjectService _projectService;


    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }


    [HttpGet("{id}")]
    public ProjectModel Get(int id)
    {
        var project = _projectService.Get(id);

        return project;
    }

    [HttpPut("{id}")]
    // [Authorize(Policy = Policies.Data.Manager)]
    public ProjectModel Put(int id, [FromBody] UpdateProjectModel model)
    {
        var projectModel = new ProjectModel
        {
            Id = id,
            Description = model.Description
        };

        var project = _projectService.Update(projectModel);

        return project;
    }
}
