using Unite.Indices.Entities.Basic.Images;

namespace Unite.Composer.Web.Resources.Domain.Basic.Images;

public class ImageResource
{
    public int Id { get; set; }
    public string ReferenceId { get; set; }
    public string Type { get; set; }

    public MrImageResource Mr { get; set; }


    public ImageResource(ImageIndex index)
    {
        Id = index.Id;
        ReferenceId = index.ReferenceId;
        Type = index.Type;

        if (index.Mr != null)
            Mr = new MrImageResource(index.Mr);
    }
}
