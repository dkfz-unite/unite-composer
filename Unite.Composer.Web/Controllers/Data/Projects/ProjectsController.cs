using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Projects;
using Unite.Composer.Data.Projects.Models;

namespace Unite.Composer.Web.Controllers.Data.Projects;

[Route("api/data/[controller]")]
[ApiController]
[Authorize]
public class ProjectsController : Controller
{
    private readonly ProjectService _projectService;


    public ProjectsController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("")]
    public ProjectModel[] Get()
    {
        var projects = _projectService.GetAll().ToArray();

        return projects;
    }
}
