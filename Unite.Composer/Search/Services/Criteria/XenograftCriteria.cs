using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria
{
    public class XenograftCriteria : SpecimenCriteriaBase
    {
        public string[] MouseStrain { get; set; }
        public string[] Intervention { get; set; }
        public Range<double?> SurvivalDays { get; set; }
        public bool? Tumorigenicity { get; set; }
        public string[] TumorGrowthForm { get; set; }

        public override bool HasValues()
        {
            return base.HasValues()
                || (Id != null && Id.Length > 0)
                || (ReferenceId != null && ReferenceId.Length > 0)
                || (MouseStrain != null && MouseStrain.Length > 0)
                || (Intervention != null && Intervention.Length > 0)
                || (SurvivalDays != null && (SurvivalDays.From.HasValue || SurvivalDays.To.HasValue))
                || Tumorigenicity.HasValue
                || (TumorGrowthForm != null && TumorGrowthForm.Length > 0);
        }
    }
}
