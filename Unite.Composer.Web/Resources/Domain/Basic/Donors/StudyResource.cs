using Unite.Indices.Entities.Basic.Projects;

namespace Unite.Composer.Web.Resources.Domain.Basic.Donors;

public class StudyResource
{
    public int Id { get; set; }
    public string Name { get; set; }


    public StudyResource(StudyNavIndex index)
    {
        Id = index.Id;
        Name = index.Name;
    }
}
