using System.Linq.Expressions;
using Unite.Data.Entities.Donors;
using Unite.Data.Entities.Images;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class ImageMappingExtensions
{
    public static ClassMap<Image> MapMrImages(this ClassMap<Image> map)
    {
        return map
            .MapDonor(entity => entity.Donor)
            .MapImage()
            .Map(entity => entity.MrImage.WholeTumor, "whole_tumor")
            .Map(entity => entity.MrImage.ContrastEnhancing, "contrast_enhancing")
            .Map(entity => entity.MrImage.NonContrastEnhancing, "non_contrast_enhancing")
            .Map(entity => entity.MrImage.MedianAdcTumor, "median_adc_tumor")
            .Map(entity => entity.MrImage.MedianAdcCe, "median_adc_ce")
            .Map(entity => entity.MrImage.MedianAdcEdema, "median_adc_edema")
            .Map(entity => entity.MrImage.MedianCbfTumor, "median_cbf_tumor")
            .Map(entity => entity.MrImage.MedianCbfCe, "median_cbf_ce")
            .Map(entity => entity.MrImage.MedianCbfEdema, "median_cbf_edema")
            .Map(entity => entity.MrImage.MedianCbvTumor, "median_cbv_tumor")
            .Map(entity => entity.MrImage.MedianCbvCe, "median_cbv_ce")
            .Map(entity => entity.MrImage.MedianCbvEdema, "median_cbv_edema")
            .Map(entity => entity.MrImage.MedianMttTumor, "median_mtt_tumor")
            .Map(entity => entity.MrImage.MedianMttCe, "median_mtt_ce")
            .Map(entity => entity.MrImage.MedianMttEdema, "median_mtt_edema");
    }


    private static ClassMap<T> MapDonor<T>(this ClassMap<T> map, Expression<Func<T, Donor>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.ReferenceId), "donor_id");
    }

    private static ClassMap<Image> MapImage(this ClassMap<Image> map)
    {
        return map
            .Map(entity => entity.ReferenceId, "image_id")
            .Map(entity => entity.TypeId, "image_type");
    }
}
