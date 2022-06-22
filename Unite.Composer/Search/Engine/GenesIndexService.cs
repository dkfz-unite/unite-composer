using Unite.Indices.Entities.Genes;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine;

public class GenesIndexService : IndexService<GeneIndex>
{
    protected override string DefaultIndex => "genes";


    public GenesIndexService(IElasticOptions options) : base(options)
    {
    }
}
