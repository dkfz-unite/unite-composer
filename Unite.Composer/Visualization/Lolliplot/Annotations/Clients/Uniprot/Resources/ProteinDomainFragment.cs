using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources;

public class ProteinDomainFragment
{
    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("end")]
    public int End { get; set; }
}
