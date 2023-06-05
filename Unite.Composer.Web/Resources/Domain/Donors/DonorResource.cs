using Unite.Composer.Data.Donors.Models;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Resources.Domain.Donors;

public class DonorResource : Basic.Donors.DonorResource
{
    public int NumberOfGenes { get; set; }
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }
    public DonorDataResource Data { get; set; }
    public AnalysedSampleModel[] AnalysedSamples { get; set; }


    public DonorResource(DonorIndex index) : base(index)
    {
        NumberOfGenes = index.NumberOfGenes;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;
        
        if (index.Data != null)
        {
            Data = new DonorDataResource(index.Data);
        }
    }
}
