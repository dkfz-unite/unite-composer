using Unite.Composer.Data.Genome.Ranges.Models.Profile;

namespace Unite.Composer.Data.Genome.Ranges.Models;

public class GenomicRangesData
{
	public IEnumerable<GenomicRange> Ranges { get; set; }
	public IEnumerable<GenesData> Genes { get; set; }
	public IEnumerable<SsmsData> Ssms { get; set; }
	public IEnumerable<CnvsData> Cnvs { get; set; }
	public IEnumerable<SvsData> Svs { get; set; }
	public IEnumerable<ExpressionData> Exps { get; set; }
	
	// public IEnumerable<GenomicRangeData> Ranges { get; set; }

	public bool HasSsms { get; set; } //=> Ranges?.Any(range => range.Ssm != null) == true;
	public bool HasCnvs { get; set; } //=> Ranges?.Any(range => range.Cnv != null) == true;
	public bool HasSvs { get; set; } //=> Ranges?.Any(range => range.Sv != null) == true;
	public bool HasExps { get; set; } //=> Ranges?.Any(range => range.Exp != null) == true;
	
	public GenomicRangesData(IEnumerable<GenomicRange> ranges)
	{
		Ranges = ranges;
	}
}
