using System.Text.Json.Serialization;

namespace Unite.Composer.Clients.Ensembl.Resources;

public abstract record LookupResource
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("version")]
    public short? Version { get; set; }
}
