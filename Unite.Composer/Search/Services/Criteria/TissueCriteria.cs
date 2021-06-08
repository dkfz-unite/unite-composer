namespace Unite.Composer.Search.Services.Criteria
{
    public class TissueCriteria : SpecimenCriteria
    {
        public string[] Type { get; set; }
        public string[] TumourType { get; set; }
        public string[] Source { get; set; }
    }
}
