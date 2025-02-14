using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Variants;

public class VariantDataResource : Basic.DataResource
{
    public VariantDataResource(DataIndex index) : base(index)
    {
    }

    public VariantDataResource(IReadOnlyDictionary<object, DataIndex> indices) : base(indices)
    {
    }
}
