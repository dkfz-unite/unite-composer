using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Search.Basic.Genome.Variants;

public class MutationResource
{
    public long Id { get; }

    public string Chromosome { get; }
    public int Start { get; }
    public int End { get; }
    public int Length { get; }

    public string Type { get; }
    public string Ref { get; }
    public string Alt { get; }


    public MutationResource(MutationIndex index)
    {
        Id = index.Id;

        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Length = index.Length;

        Type = index.Type;
        Ref = index.Ref;
        Alt = index.Alt;
    }
}
