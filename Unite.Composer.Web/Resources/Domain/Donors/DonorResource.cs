using Unite.Composer.Web.Resources.Domain.Basic;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorResource : Basic.Donors.DonorResource
{
    public int NumberOfGenes { get; set; }
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }

    public DonorDataResource Data { get; set; }
    public SampleResource[] Samples { get; set; }


    public DonorResource(DonorIndex index) : base(index)
    {
        NumberOfGenes = index.NumberOfGenes;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;
        
        if (index.Data != null)
            Data = new DonorDataResource(index.Data);

        if (index.Specimens.IsNotEmpty())
        {
            Samples = index.Specimens
                .Where(specimen => specimen.Samples.IsNotEmpty())
                .Select(specimen => new SampleResource(specimen, specimen.Samples))
                .ToArrayOrNull();
        }
    }
}
