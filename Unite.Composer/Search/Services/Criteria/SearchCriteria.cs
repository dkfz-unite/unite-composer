namespace Unite.Composer.Search.Services.Criteria
{
    public class SearchCriteria
    {
        public int From { get; set; }
        public int Size { get; set; }
        public string Term { get; set; }

        public DonorCriteria DonorFilters { get; set; }
        public TissueCriteria TissueFilters { get; set; }
        public CellLineCriteria CellLineFilters { get; set; }
        public OrganoidCriteria OrganoidFilters { get; set; }
        public XenograftCriteria XenograftFilters { get; set; }
        public MutationCriteria MutationFilters { get; set; }

        public SearchCriteria()
        {
            From = 0;
            Size = 20;
        }
    }
}
