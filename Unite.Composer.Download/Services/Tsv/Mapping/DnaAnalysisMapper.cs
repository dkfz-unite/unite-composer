using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Omics.Analysis.Dna;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class DnaAnalysisMapper
{
    public static ClassMap<TVE> GetVariantMap<TVE, TV>(bool effects)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        return new ClassMap<TVE>().MapVariantEntries<TVE, TV>(effects);
    }
}
