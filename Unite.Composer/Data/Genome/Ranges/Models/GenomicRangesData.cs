using Unite.Composer.Data.Genome.Ranges.Models.Profile;

namespace Unite.Composer.Data.Genome.Ranges.Models;

public class GenomicRangesData
{
	public IEnumerable<GenomicRange> Ranges { get; set; }
	public IEnumerable<GenesData> Genes { get; set; }
	public IEnumerable<SmsData> Sms { get; set; }
	public IEnumerable<CnvsData> Cnvs { get; set; }
	public IEnumerable<SvsData> Svs { get; set; }
	public IEnumerable<ExpressionData> Exps { get; set; }
	
	public bool HasSms { get; set; }
	public bool HasCnvs { get; set; }
	public bool HasSvs { get; set; }
	public bool HasExps { get; set; }
	
	public GenomicRangesData(IEnumerable<GenomicRange> ranges)
	{
		Ranges = ranges;
	}
}
