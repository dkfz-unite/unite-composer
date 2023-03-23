using Unite.Indices.Entities.Basic.Images;

namespace Unite.Composer.Web.Resources.Domain.Basic.Images;

public class MriImageResource
{
    public string ReferenceId { get; set; }

    public double? WholeTumor { get; set; }
    public double? ContrastEnhancing { get; set; }
    public double? NonContrastEnhancing { get; set; }

    public double? MedianAdcTumor { get; set; }
    public double? MedianAdcCe { get; set; }
    public double? MedianAdcEdema { get; set; }

    public double? MedianCbfTumor { get; set; }
    public double? MedianCbfCe { get; set; }
    public double? MedianCbfEdema { get; set; }

    public double? MedianCbvTumor { get; set; }
    public double? MedianCbvCe { get; set; }
    public double? MedianCbvEdema { get; set; }

    public double? MedianMttTumor { get; set; }
    public double? MedianMttCe { get; set; }
    public double? MedianMttEdema { get; set; }


    public MriImageResource(MriImageIndex index)
    {
        ReferenceId = index.ReferenceId;

        WholeTumor = index.WholeTumor;
        ContrastEnhancing = index.ContrastEnhancing;
        NonContrastEnhancing = index.NonContrastEnhancing;

        MedianAdcTumor = index.MedianAdcTumor;
        MedianAdcCe = index.MedianAdcCe;
        MedianAdcEdema = index.MedianAdcEdema;

        MedianCbfTumor = index.MedianCbfTumor;
        MedianCbfCe = index.MedianCbfCe;
        MedianCbfEdema = index.MedianCbfEdema;

        MedianCbvTumor = index.MedianCbvTumor;
        MedianCbvCe = index.MedianCbvCe;
        MedianCbvEdema = index.MedianCbvEdema;

        MedianMttTumor = index.MedianMttTumor;
        MedianMttCe = index.MedianMttCe;
        MedianMttEdema = index.MedianMttEdema;
    }
}
