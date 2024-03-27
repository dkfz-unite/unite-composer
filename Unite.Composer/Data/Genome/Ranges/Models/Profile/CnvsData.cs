using System.Text.Json.Serialization;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public class CnvsData : RangeData
{
    /// <summary>
    /// Variant entry.
    /// </summary>
    [JsonPropertyName("e")]
    public Cnv Variant { get; set; }

    public CnvsData(int[] range, Unite.Data.Entities.Genome.Variants.CNV.Variant variant) : base(range)
    {
        Variant = new Cnv(variant);
    }
}

public class Cnv
{
    public string Id { get; set; }
    public string Position { get; set; }
    public string Type { get; set; }
    public int Length { get; set; }
    public double? Tcn { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }
    public int? Genes { get; set; }

    public Cnv(Unite.Data.Entities.Genome.Variants.CNV.Variant variant)
    {
        Id = $"CNV{variant.Id}";
        Position = $"{variant.ChromosomeId.ToDefinitionString()}:{variant.Start}-{variant.End}";
        Type = variant.TypeId.ToDefinitionString();
        Length = variant.Length.Value;
        Tcn = variant.TcnMean;
        Loh = variant.Loh;
        Del = variant.HomoDel;
        Genes = variant.AffectedTranscripts?.DistinctBy(transcript => transcript.Feature.GeneId).Count();
    }
}
