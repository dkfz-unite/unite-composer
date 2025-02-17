using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Genes;

public class GeneDataResource : Basic.DataResource
{
    public GeneDataResource(DataIndex index) : base(index)
    {
    }

    public GeneDataResource(IReadOnlyDictionary<object, DataIndex> indices) : base(indices)
    {
    }
}
