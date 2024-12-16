using Unite.Cache.Configuration.Options;
using Unite.Cache.Repositories;
using Unite.Composer.Data.Datasets.Models;

namespace Unite.Composer.Data.Datasets.Repositories;

public class DatasetsRepository : CacheRepository<DatasetsModel>
{
    public override string DatabaseName => "user-data";
    public override string CollectionName => "datasets";

    public DatasetsRepository(IMongoOptions options) : base(options)
    {
    }
}