using Unite.Data.Entities.Genome;
using Unite.Data.Entities.Genome.Analysis.Dna;

using SSM = Unite.Data.Entities.Genome.Analysis.Dna.Ssm;
using CNV = Unite.Data.Entities.Genome.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Genome.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Tsv.Mapping.Models;

internal record VariantEntryWithAffectedFeature<TVE, TV, TVAF, TF>(TVE Entry, TVAF AffectedFeature)
        where TVE : VariantEntry<TV>
        where TV : Variant
        where TVAF : VariantAffectedFeature<TV, TF>
        where TF : Feature;

internal record SsmEntryWithAffectedTranscript : VariantEntryWithAffectedFeature<SSM.VariantEntry, SSM.Variant, SSM.AffectedTranscript, Transcript>
{
    public SsmEntryWithAffectedTranscript(SSM.VariantEntry Entry, SSM.AffectedTranscript AffectedFeature) : base(Entry, AffectedFeature) { }
}

internal record CnvEntryWithAffectedTranscript : VariantEntryWithAffectedFeature<CNV.VariantEntry, CNV.Variant, CNV.AffectedTranscript, Transcript>
{
    public CnvEntryWithAffectedTranscript(CNV.VariantEntry Entry, CNV.AffectedTranscript AffectedFeature) : base(Entry, AffectedFeature) { }
}

internal record SvEntryWithAffectedTranscript : VariantEntryWithAffectedFeature<SV.VariantEntry, SV.Variant, SV.AffectedTranscript, Transcript>
{
    public SvEntryWithAffectedTranscript(SV.VariantEntry Entry, SV.AffectedTranscript AffectedFeature) : base(Entry, AffectedFeature) { }
}
