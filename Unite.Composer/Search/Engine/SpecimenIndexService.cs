using Unite.Indices.Entities.Specimens;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine
{
    public class SpecimensIndexService : IndexService<SpecimenIndex>
    {
        protected override string DefaultIndex => "specimens";


        public SpecimensIndexService(IElasticOptions options) : base(options)
        {
        }
    }
}
