namespace Unite.Composer.Search.Services.Criteria;

public abstract class SpecimenCriteriaBase : MolecularDataCriteria
{
    public int[] Id { get; set; }
    public string[] ReferenceId { get; set; }
}
