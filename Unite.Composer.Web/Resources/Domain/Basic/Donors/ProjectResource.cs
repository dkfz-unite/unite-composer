using Unite.Indices.Entities.Basic.Projects;

namespace Unite.Composer.Web.Resources.Domain.Basic.Donors;

public class ProjectResource
{
    public int Id { get; set; }
    public string Name { get; set; }


    public ProjectResource(ProjectNavIndex index)
    {
        Id = index.Id;
        Name = index.Name;
    }
}
