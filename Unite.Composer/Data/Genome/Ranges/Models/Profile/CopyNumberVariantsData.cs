using Unite.Data.Entities.Genome.Variants.CNV.Enums;

namespace Unite.Composer.Data.Genome.Ranges.Models.Profile;

public class CopyNumberVariantsData
{
    public CnvType Cna { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }
    public double? Tcn { get; set; }
}
