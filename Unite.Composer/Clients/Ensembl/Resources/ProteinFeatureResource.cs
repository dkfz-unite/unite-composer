using System.Text.Json.Serialization;

namespace Unite.Composer.Clients.Ensembl.Resources;

public record ProteinFeatureResource
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("seqStart")]
    public int SeqStart { get; set; }

    [JsonPropertyName("seqEnd")]
    public int SeqEnd { get; set; }

    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("end")]
    public int End { get; set; }

    [JsonPropertyName("evalue")]
    public double? Evalue { get; set; }
}
