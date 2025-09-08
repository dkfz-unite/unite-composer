namespace Unite.Composer.Download.Models;

public record DataTypesCriteria
{
    public bool? Donor { get; set; }
    public bool? Treatment { get; set; }

    public bool? Specimen { get; set; }
    public bool? Intervention { get; set; }
    public bool? Drug { get; set; }

    public bool? Image { get; set; }

    public bool? Sm { get; set; }
    public bool? SmTranscript { get; set; }
    public bool? Cnv { get; set; }
    public bool? CnvTranscript { get; set; }
    public bool? Sv { get; set; }
    public bool? SvTranscript { get; set; }
    
    public bool? GeneExp { get; set; }
}
