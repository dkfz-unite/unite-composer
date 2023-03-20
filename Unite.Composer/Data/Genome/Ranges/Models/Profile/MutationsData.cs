using System.Text.Json.Serialization;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public class MutationsData
{
    [JsonPropertyName("h")]
    public int High { get; set; }

    [JsonPropertyName("m")]
    public int Moderate { get; set; }

    [JsonPropertyName("l")]
    public int Low { get; set; }

    [JsonPropertyName("u")]
    public int Unknown { get; set; }
}
