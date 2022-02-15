namespace Unite.Composer.Search.Services.Criteria
{
    public class TissueCriteria : SpecimenCriteriaBase
    {
        public string[] Type { get; set; }
        public string[] TumorType { get; set; }
        public string[] Source { get; set; }

        public override bool HasValues()
        {
            return base.HasValues()
                || (Id != null && Id.Length > 0)
                || (ReferenceId != null && ReferenceId.Length > 0)
                || (Type != null && Type.Length > 0)
                || (TumorType != null && TumorType.Length > 0)
                || (Source != null && Source.Length > 0);
        }
    }
}
