using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public record GeneCriteria
{
    public int[] Id { get; set; }

    public string[] Symbol { get; set; }
    public string[] Biotype { get; set; }
    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }
    public bool? HasMutations { get; set; }
    public bool? HasCopyNumberVariants { get; set; }
    public bool? HasStructuralVariants { get; set; }
    public bool? HasVariants { get; set; }
    public bool? HasExpressions { get; set; }


    public bool IsNotEmpty()
    {
        return Id?.Any() == true
            || Symbol?.Any() == true
            || Biotype?.Any() == true
            || Chromosome?.Any() == true
            || Position?.From != null
            || Position?.To != null
            || HasMutations == true
            || HasCopyNumberVariants == true
            || HasStructuralVariants == true
            || HasVariants == true
            || HasExpressions == true;
    }
}
