using System;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Images;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class ImageMapper
{
    public static ClassMap<Image> GetMrMap()
    {
        return new ClassMap<Image>().MapMrImages();
    }

    // public static ClassMap<Image> GetCtMap()
    // {
    //     return new ClassMap<Image>().MapCtImages();
    // }
}
