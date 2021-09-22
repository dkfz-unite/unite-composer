using System.Collections.Generic;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Services.Models
{
    public class Protein
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public int Length { get; set; }

        public IEnumerable<ProteinDomain> Domains { get; set; }
    }
}
