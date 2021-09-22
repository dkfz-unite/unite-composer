using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    public class ProteinDomainLocationResource
    {
        [XmlAttribute("start")]
        public string StartString { get; set; }

        [XmlAttribute("end")]
        public string EndString { get; set; }

        [XmlIgnore]
        public int Start => int.Parse(StartString);

        [XmlIgnore]
        public int End => int.Parse(EndString);
    }
}
