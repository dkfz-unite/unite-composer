using Unite.Composer.Data.Genome.Ranges.Models.Profile;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Data.Genome.Ranges.Models;

public class GenomicRangeData : GenomicRange
{
    public MutationsData Ssm { get; set; }
    public CopyNumberVariantsData Cnv { get; set; }
    public ExpressionsData Exp { get; set; }

	public GenomicRangeData(GenomicRange range) : base(range.Chr, range.Start, range.End)
	{

	}
}