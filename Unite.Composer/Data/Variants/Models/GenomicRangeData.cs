using System.Text.Json.Serialization;
using Unite.Composer.Data.Genome.Models;

namespace Unite.Composer.Data.Variants.Models;

public class GenomicRangeData : GenomicRange
{
    public SsmData Ssm { get; set; }
    public CnvData Cnv { get; set; }
    public ExpressionData Exp { get; set; }

	public GenomicRangeData(GenomicRange range) : base(range.Chr, range.Start, range.End)
	{

	}
}


public class SsmData
{
    [JsonPropertyName("h")]
    public int High { get; set; }

    [JsonPropertyName("m")]
    public int Moderate { get; set; }

    [JsonPropertyName("l")]
    public int Low { get; set; }

    [JsonPropertyName("u")]
    public int Unknown { get; set; }
}

public class CnvData
{
    public double? Tcn { get; set; }
    public string Cna { get; set; }
    public bool? Loh { get; set; }
    public bool? Del { get; set; }
}

public class ExpressionData
{
    public double Reads { get; set; }
    public double TPM { get; set; }
    public double FPKM { get; set; }
}