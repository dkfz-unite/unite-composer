using Unite.Indices.Entities.Basic.Genome.Dna;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome.Variants;

public class SvResource
{
    public string Id { get; set; }

    public string Chromosome { get; set; }
    public int Start { get; set; }
    public int End { get; set; }

    public string OtherChromosome { get; set; }
    public double? OtherStart { get; set; }
    public double? OtherEnd { get; set; }

    public int? Length { get; set; }

    public string Type { get; set; }
    public bool? Inverted { get; set; }
    public string FlankingSequenceFrom { get; set; }
    public string FlankingSequenceTo { get; set; }


    public SvResource(SvIndex index)
    {
        Id = index.Id;

        Chromosome = index.Chromosome;
        Start = index.Start;
        End = index.End;

        OtherChromosome = index.OtherChromosome;
        OtherStart = index.OtherStart;
        OtherEnd = index.OtherEnd;

        Length = index.Length;

        Type = index.Type;
        Inverted = index.Inverted;
    }
}
