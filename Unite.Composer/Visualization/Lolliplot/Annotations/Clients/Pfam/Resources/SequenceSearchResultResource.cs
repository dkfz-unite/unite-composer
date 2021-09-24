using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    [XmlRoot(ElementName = "pfam", Namespace = "https://pfam.xfam.org/")]
    public class SequenceSearchResultResource
    {
        [XmlElement("results")]
        public JobResult Result { get; set; }
    }

    public class JobResult
    {
        [XmlAttribute("job_id")]
        public string Id { get; set; }

        //[XmlArray("matches")]
        //[XmlArrayItem("protein")]
        //public Protein[] Proteins { get; set; }

        [XmlElement("matches")]
        public Match Match { get; set; }
    }

    public class Match
    {
        [XmlElement("protein")]
        public Protein Protein { get; set; }
    }

    public class Protein
    {
        [XmlAttribute("length")]
        public int Length { get; set; }

        [XmlArray("database")]
        [XmlArrayItem("match")]
        public ProteinDomain[] Domains { get; set; }
    }

    public class ProteinDomain
    {
        [XmlAttribute("accession")]
        public string Id { get; set; }

        [XmlAttribute("id")]
        public string Symbol { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }


        [XmlElement("location")]
        public ProteinDomainLocation Location { get; set; }
    }

    public class ProteinDomainLocation
    {
        [XmlAttribute("start")]
        public int Start { get; set; }

        [XmlAttribute("end")]
        public int End { get; set; }
    }
}
