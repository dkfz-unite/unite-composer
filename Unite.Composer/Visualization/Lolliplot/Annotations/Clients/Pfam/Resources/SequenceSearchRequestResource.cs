using System;
using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources
{
    [XmlRoot(ElementName = "jobs", Namespace = "https://pfam.xfam.org/")]
    public class SequenceSearchRequestResource
    {
        [XmlElement("job")]
        public Job Job { get; set; }
    }

    public class Job
    {
        [XmlAttribute("job_id")]
        public string Id { get; set; }

        [XmlElement("opened")]
        public DateTime Date { get; set; }

        [XmlElement("result_url")]
        public string ResultUrl { get; set; }
    }
}
