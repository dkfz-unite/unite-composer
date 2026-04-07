using Unite.Indices.Entities.Proteins;

namespace Unite.Composer.Web.Resources.Domain.Proteins;

public class ProteinResource : Basic.Omics.ProteinResource
{
    public ProteinStatsResource Stats { get; set; }
    public ProteinDataResource Data { get; set; }

    public Basic.Omics.TranscriptResourceBase Transcript { get; set; }
    public Basic.Omics.GeneResourceBase Gene { get; set; }


    public ProteinResource(ProteinIndex index) : base(index)
    {
        if (index.Stats != null)
            Stats = new ProteinStatsResource(index.Stats);

        if (index.Data != null)
            Data = new ProteinDataResource(index.Data);

        Transcript = new Basic.Omics.TranscriptResourceBase
        {
            Id = index.Transcript.Id,
            StableId = index.Transcript.StableId,
            Symbol = index.Transcript.Symbol
        };

        Gene = new Basic.Omics.GeneResourceBase
        {
            Id = index.Gene.Id,
            StableId = index.Gene.StableId,
            Symbol = index.Gene.Symbol
        };
    }
}
