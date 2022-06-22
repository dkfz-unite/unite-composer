using Unite.Indices.Entities.Images;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Search.Engine;

public class ImagesIndexService : IndexService<ImageIndex>
{
    protected override string DefaultIndex => "images";


    public ImagesIndexService(IElasticOptions options) : base(options)
    {
    }
}
