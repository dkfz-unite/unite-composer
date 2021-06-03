using Unite.Indices.Entities.Donors;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine
{
    public class DonorsIndexService : IndexService<DonorIndex>
    {
        protected override string DefaultIndex => "donors";


        public DonorsIndexService(IElasticOptions options) : base(options)
        {
        }
    }
}
