namespace Unite.Composer.Data.Variants.Models;

public class GenomicRangesData
{
	public int Ploidy { get; set; } = 2;
	public IEnumerable<GenomicRangeData> Ranges { get; set; }

	public bool HasSsm => Ranges?.Any(range => range.Ssm != null) == true;
	public bool HasCnv => Ranges?.Any(range => range.Cnv != null) == true;
	public bool HasExp => Ranges?.Any(range => range.Exp != null) == true;
}
