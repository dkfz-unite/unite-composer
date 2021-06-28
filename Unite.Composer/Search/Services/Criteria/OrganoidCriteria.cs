namespace Unite.Composer.Search.Services.Criteria
{
    public class OrganoidCriteria : SpecimenCriteria
    {
        public string[] Medium { get; set; }
        public string[] Intervention { get; set; }
        public bool? Tumorigenicity { get; set; }
    }
}
