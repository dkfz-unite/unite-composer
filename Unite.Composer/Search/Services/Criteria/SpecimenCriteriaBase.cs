using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public abstract class SpecimenCriteriaBase : MolecularDataCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }

    public string[] Drug { get; set; }
    public Range<double?> Dss { get; set; }
    public Range<double?> DssSelective { get; set; }
}
