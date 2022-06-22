namespace Unite.Composer.Search.Services.Criteria;

public class OrganoidCriteria : SpecimenCriteriaBase
{
    public string[] Medium { get; set; }
    public string[] Intervention { get; set; }
    public bool? Tumorigenicity { get; set; }
}
