using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria
{
    public class XenograftCriteria : SpecimenCriteria
    {
        public string[] MouseStrain { get; set; }
        public bool? Tumorigenicity { get; set; }
        public string[] TumorGrowthForm { get; set; }
        public Range<double?> SurvivalDays { get; set; }

        public string[] Intervention { get; set; }
    }
}
