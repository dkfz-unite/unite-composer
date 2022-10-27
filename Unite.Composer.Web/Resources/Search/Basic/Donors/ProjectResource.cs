using Unite.Indices.Entities.Basic.Donors;

namespace Unite.Composer.Web.Resources.Search.Basic.Donors;

public class ProjectResource
{
    public int Id { get; set; }
    public string Name { get; set; }


    public ProjectResource(ProjectIndex index)
    {
        Id = index.Id;
        Name = index.Name;
    }
}
