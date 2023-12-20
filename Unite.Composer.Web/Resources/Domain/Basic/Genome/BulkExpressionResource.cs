using Unite.Indices.Entities.Basic.Genome.Transcriptomics;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome;

public class BulkExpressionResource
{
    public int Reads { get; set; }
    public double TPM { get; set; }
    public double FPKM { get; set; }

    public BulkExpressionResource(BulkExpressionIndex index){
        Reads = index.Reads;
        TPM = index.Tpm;
        FPKM = index.Fpkm;
    }
}
