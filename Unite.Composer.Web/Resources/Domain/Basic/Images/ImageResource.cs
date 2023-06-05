using Unite.Indices.Entities.Basic.Images;

namespace Unite.Composer.Web.Resources.Domain.Basic.Images;

public class ImageResource
{
    public int Id { get; set; }
    public int? ScanningDay { get; set; }

    public MriImageResource Mri { get; set; }


    public ImageResource(ImageIndex index)
    {
        Id = index.Id;
        ScanningDay = index.ScanningDay;

        if (index.Mri != null)
        {
            Mri = new MriImageResource(index.Mri);
        }
    }
}
