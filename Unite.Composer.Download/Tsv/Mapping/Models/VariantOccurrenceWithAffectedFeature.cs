using Unite.Data.Entities.Genome;
using Unite.Data.Entities.Genome.Variants;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv.Mapping.Models;

internal record VariantOccurrenceWithAffectedFeature<TVO, TV, TVAF, TF>(TVO Occurrence, TVAF AffectedFeature)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
        where TVAF : VariantAffectedFeature<TV, TF>
        where TF : Feature;

internal record SsmOccurrenceWithAffectedTranscript : VariantOccurrenceWithAffectedFeature<SSM.VariantOccurrence, SSM.Variant, SSM.AffectedTranscript, Transcript>
{
    public SsmOccurrenceWithAffectedTranscript(SSM.VariantOccurrence Occurrence, SSM.AffectedTranscript AffectedFeature) : base(Occurrence, AffectedFeature) { }
}

internal record CnvOccurrenceWithAffectedTranscript : VariantOccurrenceWithAffectedFeature<CNV.VariantOccurrence, CNV.Variant, CNV.AffectedTranscript, Transcript>
{
    public CnvOccurrenceWithAffectedTranscript(CNV.VariantOccurrence Occurrence, CNV.AffectedTranscript AffectedFeature) : base(Occurrence, AffectedFeature) { }
}

internal record SvOccurrenceWithAffectedTranscript : VariantOccurrenceWithAffectedFeature<SV.VariantOccurrence, SV.Variant, SV.AffectedTranscript, Transcript>
{
    public SvOccurrenceWithAffectedTranscript(SV.VariantOccurrence Occurrence, SV.AffectedTranscript AffectedFeature) : base(Occurrence, AffectedFeature) { }
}
