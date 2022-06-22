using Unite.Indices.Entities.Basic.Images;

namespace Unite.Composer.Web.Resources.Images;

public class ImageBaseResource
{
    public int Id { get; set; }
    public int? ScanningDay { get; set; }

    public MriImageResource MriImage { get; set; }


    public ImageBaseResource(ImageIndex index)
    {
        Id = index.Id;
        ScanningDay = index.ScanningDay;

        if (index.MriImage != null)
        {
            MriImage = new MriImageResource(index.MriImage);
        }
    }
}
