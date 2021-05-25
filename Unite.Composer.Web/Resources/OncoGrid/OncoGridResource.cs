using System.Collections.Generic;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Web.Resources.Donors;

namespace Unite.Composer.Web.Resources.OncoGrid
{
    public class OncoGridResource
    {
        public List<ObservationResource> Observations { get; } = new();
        public List<GeneResource> Genes { get; } = new();
        public List<DonorResource> Donors { get; } = new();
    }
}