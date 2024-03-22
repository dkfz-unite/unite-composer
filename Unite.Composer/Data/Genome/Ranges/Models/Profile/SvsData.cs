using System.Text.Json.Serialization;
using Unite.Essentials.Extensions;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public class SvsData : RangeData
{
    /// <summary>
    /// Variant entry.
    /// </summary> 
    [JsonPropertyName("e")]
    public Sv Variant { get; set; }

    public SvsData(int[] range, Unite.Data.Entities.Genome.Variants.SV.Variant variant) : base(range)
    {
        Variant = new Sv(variant);
    }
}

public class Sv
{
    public string Id { get; set; }
    public string Position { get; set; }
    public string Type { get; set; }
    public int Length { get; set; }

    public Sv(Unite.Data.Entities.Genome.Variants.SV.Variant variant)
    {
        Id = $"SV{variant.Id}";
        Position = $"{variant.ChromosomeId.ToDefinitionString()}:{variant.End}-{variant.OtherStart}";
        Type = variant.TypeId.ToDefinitionString();
        Length = variant.Length.Value;
    }
}
