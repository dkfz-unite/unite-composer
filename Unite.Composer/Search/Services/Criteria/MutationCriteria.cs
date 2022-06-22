using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public class MutationCriteria
{
    public long[] Id { get; set; }

    public string[] Code { get; set; }
    public string[] MutationType { get; set; }
    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }
    public string[] Impact { get; set; }
    public string[] Consequence { get; set; }
}
