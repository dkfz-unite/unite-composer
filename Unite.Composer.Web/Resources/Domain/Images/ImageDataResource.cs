using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageDataResource : Basic.DataResource
{
    public ImageDataResource(DataIndex index) : base(index)
    {
    }

    public ImageDataResource(IReadOnlyDictionary<object, DataIndex> indices) : base(indices)
    {
    }
}
