namespace Unite.Composer.Search.Services.Criteria;

public record CellLineCriteria : SpecimenCriteriaBase
{
    public string[] Species { get; set; }
    public string[] Type { get; set; }
    public string[] CultureType { get; set; }

    public string[] Name { get; set; }
}
