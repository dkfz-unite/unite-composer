using Unite.Composer.Indices.Criteria.Filters;

namespace Unite.Composer.Indices.Criteria
{
    public class SearchCriteria
    {
        public int From { get; set; }
        public int Size { get; set; }
        public string Term { get; set; }

        public DonorFilters DonorFilters { get; set; }
        public CellLineFilters CellLineFilters { get; set; }
        public MutationFilters MutationFilters { get; set; }
        public OncoGridFilters OncoGridFilters { get; set; }

        public SearchCriteria()
        {
            From = 0;
            Size = 20;
        }
    }
}
