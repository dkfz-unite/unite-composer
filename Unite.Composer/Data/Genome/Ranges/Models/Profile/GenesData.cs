using System.Text.Json.Serialization;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public class GenesData : RangeData
{
    /// <summary>
    /// Gene entry.
    /// </summary>
    [JsonPropertyName("e")]
    public Gene Gene { get; set; }

    public GenesData(int[] range, Unite.Data.Entities.Genome.Gene gene) : base(range)
    {
        Gene = new Gene(gene);
    }
}

public class Gene
{
    public string Id { get; set; }
    public string Position { get; set; }
    public bool Strand { get; set; }
    public string Symbol { get; set; }
    public string Biotype { get; set; }
    public int Length { get; set; }
    public int LengthExons { get; set; }


    public Gene(Unite.Data.Entities.Genome.Gene gene)
    {
        Id = $"{gene.Id}";
        Position = $"{gene.ChromosomeId.ToDefinitionString()}:{gene.Start}-{gene.End}";
        Strand = gene.Strand.Value;
        Symbol = gene.Symbol;
        Biotype = gene.Biotype;
        Length = gene.End.Value - gene.Start.Value + 1;
        LengthExons = gene.ExonicLength.Value;
    }
}
