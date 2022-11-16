using Unite.Composer.Search.Services.Criteria.Models;

namespace Unite.Composer.Search.Services.Criteria;

public class GeneCriteria
{
    public int[] Id { get; set; }

    public string[] Symbol { get; set; }
    public string[] Biotype { get; set; }
    public string[] Chromosome { get; set; }
    public Range<double?> Position { get; set; }
    public bool? HasMutations { get; set; }
    public bool? HasCopyNumberVariants { get; set; }
    public bool? HasStructuralVariants { get; set; }
}
