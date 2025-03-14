using Unite.Indices.Entities.Projects;

namespace Unite.Composer.Web.Resources.Domain.Projects;

public class ProjectResource : ProjectIndex
{
    public ProjectResource(ProjectIndex index)
    {
        Id = index.Id;
        Name = index.Name;
        Stats = index.Stats;
        Data = index.Data;
        // Donors = index.Donors;
    }
}
