namespace Unite.Composer.Data.Genome.Ranges.Models;

public class GenomicRangesData
{
	public IEnumerable<GenomicRangeData> Ranges { get; set; }

	public bool HasSsm => Ranges?.Any(range => range.Ssm != null) == true;
	public bool HasCnv => Ranges?.Any(range => range.Cnv != null) == true;
	public bool HasExp => Ranges?.Any(range => range.Exp != null) == true;

	public GenomicRangesData(IEnumerable<GenomicRangeData> ranges)
	{
		Ranges = ranges;
	}
}
