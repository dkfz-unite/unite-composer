using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Domain.Basic.Specimens;

public class OrganoidResource : SpecimenBaseResource
{
    public string Medium { get; set; }
    public int? ImplantedCellsNumber { get; set; }
    public bool? Tumorigenicity { get; set; }

    
    public OrganoidResource(OrganoidIndex index) : base(index)
    {
        Medium = index.Medium;
        ImplantedCellsNumber = index.ImplantedCellsNumber;
        Tumorigenicity = index.Tumorigenicity;
    }
}
