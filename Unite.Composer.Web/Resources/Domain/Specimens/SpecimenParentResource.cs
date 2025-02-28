using Unite.Indices.Entities.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Specimens;

public class SpecimenParentResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }

    public SpecimenParentResource(ParentIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        Type = index.Type;
    }
}
