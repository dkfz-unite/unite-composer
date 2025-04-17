namespace Unite.Composer.Download.Tsv.Models;

public record DataTypesCriteria
{
    public bool? Donors { get; set; }
    public bool? Clinical { get; set; }
    public bool? Treatments { get; set; }

    public bool? Specimens { get; set; }
    public bool? Molecular { get; set; }
    public bool? Interventions { get; set; }
    public bool? Drugs { get; set; }

    public bool? Mrs { get; set; }
    public bool? Cts { get; set; }

    public bool? Sms { get; set; }
    public bool? SmsTranscriptsSlim { get; set; }
    public bool? SmsTranscriptsFull { get; set; }
    public bool? Cnvs { get; set; }
    public bool? CnvsTranscriptsSlim { get; set; }
    public bool? CnvsTranscriptsFull { get; set; }
    public bool? Svs { get; set; }
    public bool? SvsTranscriptsSlim { get; set; }
    public bool? SvsTranscriptsFull { get; set; }
    
    public bool? GeneExp { get; set; }
    public bool? GeneExpSc { get; set; }
}
