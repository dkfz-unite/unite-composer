using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public abstract class VariantCriteriaBase
{
    public string[] Id { get; set; }

    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }

    public string[] Impact { get; set; }
    public string[] Consequence { get; set; }
}
