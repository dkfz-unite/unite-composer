using Unite.Indices.Entities;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorDataResource : Basic.DataResource
{
    public DonorDataResource(DataIndex index) : base(index)
    {
    }

    public DonorDataResource(IReadOnlyDictionary<object, DataIndex> indices) : base(indices)
    {
    }
}
