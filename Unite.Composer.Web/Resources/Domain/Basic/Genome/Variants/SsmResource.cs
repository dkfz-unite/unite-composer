using Unite.Indices.Entities.Basic.Genome.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class SsmResource
{
    public long Id { get; }

    public string Chromosome { get; }
    public int Start { get; }
    public int End { get; }
    public int Length { get; }

    public string Type { get; }
    public string Ref { get; }
    public string Alt { get; }


    public SsmResource(SsmIndex index)
    {
        Id = index.Id;

        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;
        Length = index.Length.Value;

        Type = index.Type;
        Ref = index.Ref;
        Alt = index.Alt;
    }
}
