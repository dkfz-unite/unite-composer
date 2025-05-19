using Unite.Essentials.Extensions;
using Unite.Indices.Entities.Basic.Omics.Variants;

namespace Unite.Composer.Web.Resources.Domain.Basic.Omics.Variants;

public class AffectedFeatureResource
{
    public GeneResource Gene { get; set; }

    public AffectedTranscriptResource Transcript { get; set; }
    //public AffectedRegulatorResource Regulator { get; set; }
    //public AffectedMotifResource Motif { get; set; }

    public EffectResource[] Effects { get; set; }


    public AffectedFeatureResource(AffectedFeatureIndex index)
    {
        if (index.Gene != null)
            Gene = new GeneResource(index.Gene);

        if (index.Transcript != null)
            Transcript = new AffectedTranscriptResource(index.Transcript);

        if (index.Effects.IsNotEmpty())
        {
            Effects = index.Effects
                .Select(effectIndex => new EffectResource(effectIndex))
                .ToArray();
        }
    }
}
