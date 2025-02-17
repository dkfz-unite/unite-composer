using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorResource : Basic.Donors.DonorResource
{
    public DonorStatsResource Stats { get; set; }
    public DonorDataResource Data { get; set; }
    public DonorSampleResource[] Samples { get; set; }


    public DonorResource(DonorIndex index) : base(index)
    {
        if (index.Stats != null)
            Stats = new DonorStatsResource(index.Stats);
        
        if (index.Data != null)
            Data = new DonorDataResource(index.Data);

        if (index.Specimens.IsNotEmpty())
        {
            Samples = index.Specimens
                .Where(specimen => specimen.Samples.IsNotEmpty())
                .Select(specimen => new DonorSampleResource(specimen, specimen.Samples))
                .ToArrayOrNull();
        }
    }
}
