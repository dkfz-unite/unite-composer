using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class MaterialResource : SpecimenBaseResource
{
    public string FixationType { get; set; }
    public string Source { get; set; }

    public MaterialResource(MaterialIndex index) : base(index)
    {
        FixationType = index.FixationType;
        Source = index.Source;
    }
}
