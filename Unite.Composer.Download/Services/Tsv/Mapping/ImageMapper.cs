using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Images.Enums;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class ImageMapper
{
    public static ClassMap<Image> GetImageMap(ImageType type)
    {
        if (type == ImageType.MR)
            return GetMrMap();
        // else if (type == ImageType.CT)
        //     return GetCtMap();
        else
            throw new NotSupportedException($"Image type '{type}' is not supported.");
    }

    public static ClassMap<Image> GetMrMap()
            {
                return new ClassMap<Image>().MapMrImages();
            }

    // public static ClassMap<Image> GetCtMap()
    // {
    //     return new ClassMap<Image>().MapCtImages();
    // }
}
