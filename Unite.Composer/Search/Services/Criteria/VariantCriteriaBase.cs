using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public abstract record VariantCriteriaBase
{
    public string[] Id { get; set; }

    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }
    public Range<int?> Length { get; set; }

    public string[] Impact { get; set; }
    public string[] Consequence { get; set; }


    public virtual bool IsNotEmpty()
    {
        return Id?.Any() == true
            || Chromosome?.Any() == true
            || Position?.From != null
            || Position?.To != null
            || Length?.From != null
            || Length?.To != null
            || Impact?.Any() == true
            || Consequence?.Any() == true;
    }
}
