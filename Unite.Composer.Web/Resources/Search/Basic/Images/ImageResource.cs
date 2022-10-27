using Unite.Indices.Entities.Basic.Images;

namespace Unite.Composer.Web.Resources.Search.Basic.Images;

public class ImageResource
{
    public int Id { get; set; }
    public int? ScanningDay { get; set; }

    public MriImageResource MriImage { get; set; }


    public ImageResource(ImageIndex index)
    {
        Id = index.Id;
        ScanningDay = index.ScanningDay;

        if (index.MriImage != null)
        {
            MriImage = new MriImageResource(index.MriImage);
        }
    }
}
