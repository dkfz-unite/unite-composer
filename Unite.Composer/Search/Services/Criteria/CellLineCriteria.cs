namespace Unite.Composer.Search.Services.Criteria
{
    public class CellLineCriteria : SpecimenCriteriaBase
	{
		public string[] Species { get; set; }
		public string[] Type { get; set; }
		public string[] CultureType { get; set; }

		public string[] Name { get; set; }

        public override bool HasValues()
        {
            return base.HasValues()
                || (Id != null && Id.Length > 0)
                || (ReferenceId != null && ReferenceId.Length > 0)
                || (Species != null && Species.Length > 0)
                || (Type != null && Type.Length > 0)
                || (CultureType != null && CultureType.Length > 0)
                || (Name != null && Name.Length > 0);
        }
    }
}
