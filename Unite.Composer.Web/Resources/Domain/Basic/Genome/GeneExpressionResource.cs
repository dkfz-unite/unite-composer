using Unite.Indices.Entities.Basic.Genome.Transcriptomics;

namespace Unite.Composer.Web.Resources.Domain.Basic.Genome;

public class GeneExpressionResource
{
    public int Reads { get; set; }
    public double TPM { get; set; }
    public double FPKM { get; set; }

    public GeneExpressionResource(GeneExpressionIndex index){
        Reads = index.Reads;
        TPM = index.Tpm;
        FPKM = index.Fpkm;
    }
}
