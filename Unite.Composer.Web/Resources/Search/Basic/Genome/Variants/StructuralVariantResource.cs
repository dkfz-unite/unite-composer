using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome.Variants;

public class StructuralVariantResource
{
    public long Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public string NewChromosome { get; set; }
    public double? NewStart { get; set; }
    public double? NewEnd { get; set; }

    public string Type { get; set; }
    public string Ref { get; set; }
    public string Alt { get; set; }


    public StructuralVariantResource(StructuralVariantIndex index)
    {
        Id = index.Id;

        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;

        NewChromosome = index.NewChromosome;
        NewStart = index.NewStart;
        NewEnd = index.NewEnd;

        Type = index.Type;
        Ref = index.Ref;
        Alt = index.Alt;
    }
}
