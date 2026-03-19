using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Proteins;

public class ProteinDataResource : Basic.DataResource
{
    public ProteinDataResource(DataIndex index) : base(index)
    {
    }

    public ProteinDataResource(IReadOnlyDictionary<object, DataIndex> indices) : base(indices)
    {
    }
}
