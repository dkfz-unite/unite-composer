namespace Unite.Composer.Download.Models;

public class DownloadCriteria
{
    public bool? Donor { get; set; }
    public bool? Treatment { get; set; }

    public bool? Specimen { get; set; }
    public bool? Intervention { get; set; }
    public bool? Drug { get; set; }

    public bool? Image { get; set; }

    public bool? Sm { get; set; }
    public bool? SmEffect { get; set; }
    public bool? Cnv { get; set; }
    public bool? CnvEffect { get; set; }
    public bool? Sv { get; set; }
    public bool? SvEffect { get; set; }
    
    public bool? GeneExp { get; set; }
}
