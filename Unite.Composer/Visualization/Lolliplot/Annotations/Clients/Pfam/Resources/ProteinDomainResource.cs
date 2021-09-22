using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    public class ProteinDomainResource
    {
        [XmlAttribute("accession")]
        public string Id { get; set; }

        [XmlAttribute("id")]
        public string Symbol { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }


        [XmlElement("location")]
        public ProteinDomainLocationResource Location { get; set; }
    }
}
