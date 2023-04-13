namespace Unite.Composer.Search.Services.Criteria;

public record TissueCriteria : SpecimenCriteriaBase
{
    public string[] Type { get; set; }
    public string[] TumorType { get; set; }
    public string[] Source { get; set; }
}
