using Unite.Composer.Indices.Criteria.Filters.Standard;

namespace Unite.Composer.Indices.Criteria.Filters
{
    public class MutationFilters
    {
        public string[] Code { get; set; }
        public string[] MutationType { get; set; }
        public string[] Chromosome { get; set; }
        public Range Position { get; set; }
        public string[] Impact { get; set; }
        public string[] Consequence { get; set; }
        public string[] Gene { get; set; }
    }
}
