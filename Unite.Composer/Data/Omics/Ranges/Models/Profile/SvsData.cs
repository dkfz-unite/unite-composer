using System.Text.Json.Serialization;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Omics.Ranges.Models.Profile;

public class SvsData : RangeData
{
    /// <summary>
    /// Variant entry.
    /// </summary> 
    [JsonPropertyName("e")]
    public Sv Variant { get; set; }

    public SvsData(int[] range, Unite.Data.Entities.Omics.Analysis.Dna.Sv.Variant variant) : base(range)
    {
        Variant = new Sv(variant);
    }
}

public class Sv
{
    public string Id { get; set; }
    public string Position { get; set; }
    public string Type { get; set; }
    public int? Length { get; set; }
    public int? Genes { get; set; }

    public Sv(Unite.Data.Entities.Omics.Analysis.Dna.Sv.Variant variant)
    {
        Id = $"SV{variant.Id}";
        Position = GetPosition(variant);
        Type = variant.TypeId.ToDefinitionString();
        Length = variant.Length;
        Genes = variant.AffectedTranscripts?.DistinctBy(transcript => transcript.Feature.GeneId).Count();
    }

    private static string GetPosition(Unite.Data.Entities.Omics.Analysis.Dna.Sv.Variant variant)
    {
        if (variant.ChromosomeId == variant.OtherChromosomeId)
            return $"{variant.ChromosomeId.ToDefinitionString()}:{variant.End}-{variant.OtherStart}";
        else
            return $"{variant.ChromosomeId.ToDefinitionString()}:{variant.End} - {variant.OtherChromosomeId.ToDefinitionString()}:{variant.OtherStart}";
    }
}
