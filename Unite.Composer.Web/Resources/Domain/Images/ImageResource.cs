using Unite.Composer.Web.Resources.Domain.Basic;
using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Images;

namespace Unite.Composer.Web.Resources.Domain.Images;

public class ImageResource : Basic.Images.ImageResource
{
    public int DonorId { get; set; }

    public int NumberOfGenes { get; set; }
    public int NumberOfSsms { get; set; }
    public int NumberOfCnvs { get; set; }
    public int NumberOfSvs { get; set; }

    public ImageDataResource Data { get; set; }
    public SampleResource[] Samples { get; set; }


    public ImageResource(ImageIndex index) : base(index)
    {
        DonorId = index.DonorId;

        NumberOfGenes = index.NumberOfGenes;
        NumberOfSsms = index.NumberOfSsms;
        NumberOfCnvs = index.NumberOfCnvs;
        NumberOfSvs = index.NumberOfSvs;

        if (index.Data != null)
            Data = new ImageDataResource(index.Data);

        if (index.Specimens.IsNotEmpty())
        {
            Samples = index.Specimens
                .Where(specimen => specimen.Analyses.IsNotEmpty())
                .Select(specimen => new SampleResource(specimen, specimen.Analyses))
                .ToArrayOrNull();
        }
    }
}
