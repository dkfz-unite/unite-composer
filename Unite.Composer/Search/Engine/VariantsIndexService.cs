using Unite.Indices.Entities.Variants;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine;

public class VariantsIndexService : IndexService<VariantIndex>
{
    protected override string DefaultIndex => "variants";


    public VariantsIndexService(IElasticOptions options) : base(options)
    {
    }
}
