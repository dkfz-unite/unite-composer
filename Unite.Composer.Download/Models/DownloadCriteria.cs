namespace Unite.Composer.Download.Models;

public class DownloadCriteria
{
    public bool? Donor { get; set; }
    public bool? Treatment { get; set; }

    public bool? Specimen { get; set; }
    public bool? Intervention { get; set; }
    public bool? Drug { get; set; }

    public bool? Mr { get; set; }
    public bool? Ct { get; set; }

    public bool? Sm { get; set; }
    public bool? SmEffectSlim { get; set; }
    public bool? SmEffectFull { get; set; }
    public bool? Cnv { get; set; }
    public bool? CnvEffectSlim { get; set; }
    public bool? CnvEffectFull { get; set; }
    public bool? Sv { get; set; }
    public bool? SvEffectSlim { get; set; }
    public bool? SvEffectFull { get; set; }
    
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }
}
