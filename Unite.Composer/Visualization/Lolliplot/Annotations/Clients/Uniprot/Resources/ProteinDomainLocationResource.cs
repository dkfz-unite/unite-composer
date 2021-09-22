using System.Text.Json.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Resources
{
    public class ProteinDomainLocationResource
    {
        [JsonPropertyName("fragments")]
        public ProteinDomainFragmentResource[] Fragments { get; set; }
    }
}
