using Unite.Data.Entities.Omics;
using Unite.Data.Entities.Omics.Analysis.Dna;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Tsv.Mapping.Models;

internal record VariantEntryWithAffectedFeature<TVE, TV, TVAF, TF>(TVE Entry, TVAF AffectedFeature)
        where TVE : VariantEntry<TV>
        where TV : Variant
        where TVAF : VariantAffectedFeature<TV, TF>
        where TF : Feature;

internal record SmEntryWithAffectedTranscript : VariantEntryWithAffectedFeature<SM.VariantEntry, SM.Variant, SM.AffectedTranscript, Transcript>
{
    public SmEntryWithAffectedTranscript(SM.VariantEntry Entry, SM.AffectedTranscript AffectedFeature) : base(Entry, AffectedFeature) { }
}

internal record CnvEntryWithAffectedTranscript : VariantEntryWithAffectedFeature<CNV.VariantEntry, CNV.Variant, CNV.AffectedTranscript, Transcript>
{
    public CnvEntryWithAffectedTranscript(CNV.VariantEntry Entry, CNV.AffectedTranscript AffectedFeature) : base(Entry, AffectedFeature) { }
}

internal record SvEntryWithAffectedTranscript : VariantEntryWithAffectedFeature<SV.VariantEntry, SV.Variant, SV.AffectedTranscript, Transcript>
{
    public SvEntryWithAffectedTranscript(SV.VariantEntry Entry, SV.AffectedTranscript AffectedFeature) : base(Entry, AffectedFeature) { }
}
