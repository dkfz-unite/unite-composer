using Unite.Composer.Data.Omics.Ranges.Models.Profile;

namespace Unite.Composer.Data.Omics.Ranges.Models;

public class GenomicRangesData
{
	public IEnumerable<GenomicRange> Ranges { get; set; }
	public IEnumerable<GenesData> Genes { get; set; }
	public IEnumerable<ProteinsData> Prots { get; set; }
	public IEnumerable<ProteinsData> Proteins { get; set; }
	public IEnumerable<SmsData> Sms { get; set; }
	public IEnumerable<CnvsData> Cnvs { get; set; }
	public IEnumerable<SvsData> Svs { get; set; }
	public IEnumerable<GeneExpressionData> Exps { get; set; }
	public IEnumerable<ProteinExpressionData> Pexps { get; set; }
	
	public bool HasSms { get; set; }
	public bool HasCnvs { get; set; }
	public bool HasSvs { get; set; }
	public bool HasExps { get; set; }
	public bool HasProts { get; set; }
	
	public GenomicRangesData(IEnumerable<GenomicRange> ranges)
	{
		Ranges = ranges;
	}
}
