using Unite.Indices.Entities.Mutations;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine;

public class MutationsIndexService : IndexService<MutationIndex>
{
    protected override string DefaultIndex => "mutations";


    public MutationsIndexService(IElasticOptions options) : base(options)
    {
    }
}
