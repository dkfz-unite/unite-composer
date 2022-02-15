namespace Unite.Composer.Search.Services.Criteria
{
    public class OrganoidCriteria : SpecimenCriteriaBase
    {
        public string[] Medium { get; set; }
        public string[] Intervention { get; set; }
        public bool? Tumorigenicity { get; set; }

        public override bool HasValues()
        {
            return base.HasValues()
                || (Id != null && Id.Length > 0)
                || (ReferenceId != null && ReferenceId.Length > 0)
                || (Medium != null && Medium.Length > 0)
                || (Intervention != null && Intervention.Length > 0)
                || Tumorigenicity.HasValue;
        }
    }
}
