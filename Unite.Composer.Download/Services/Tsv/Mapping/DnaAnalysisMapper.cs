using System.Linq.Expressions;
using Unite.Composer.Download.Services.Tsv.Mapping.Converters;
using Unite.Data.Entities.Omics.Analysis.Dna;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Services.Tsv.Mapping;

public static class DnaAnalysisMapper
{
    public static ClassMap<TVE> GetVariantMap<TVE, TV>(bool effects)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        return new ClassMap<TVE>().MapVariantEntries<TVE, TV>(effects);
    }

    public static ClassMap<TVE> MapVariantEntries<TVE, TV>(this ClassMap<TVE> map, bool transcripts = false)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        if (map is ClassMap<SM.VariantEntry> ssmMap)
            ssmMap.MapVariantEntries(transcripts);
        else if (map is ClassMap<CNV.VariantEntry> cnvMap)
            cnvMap.MapVariantEntries(transcripts);
        else if (map is ClassMap<SV.VariantEntry> svMap)
            svMap.MapVariantEntries(transcripts);

        return map;
    }

    public static ClassMap<SM.VariantEntry> MapVariantEntries(this ClassMap<SM.VariantEntry> map, bool transcripts = false)
    {
        return map.MapVariant(entity => entity.Entity)
                  .MapAffectedFeatures(transcripts);
    }

    public static ClassMap<CNV.VariantEntry> MapVariantEntries(this ClassMap<CNV.VariantEntry> map, bool transcripts = false)
    {
        return map.MapVariant(entity => entity.Entity)
                  .MapAffectedFeatures(transcripts);
    }

    public static ClassMap<SV.VariantEntry> MapVariantEntries(this ClassMap<SV.VariantEntry> map, bool transcripts = false)
    {
        return map.MapVariant(entity => entity.Entity)
                  .MapAffectedFeatures(transcripts);
    }


    private static ClassMap<T> MapVariant<T>(this ClassMap<T> map, Expression<Func<T, SM.Variant>> path) where T : class
    {
        var chromosomeConverter = new ChromosomeConverter();

        return map
            .Map(path.Join(entity => entity.ChromosomeId), "chromosome", chromosomeConverter)
            .Map(path.Join(entity => entity.Start), "start")
            .Map(path.Join(entity => entity.End), "end")
            .Map(path.Join(entity => entity.Length), "length")
            .Map(path.Join(entity => entity.Ref), "ref")
            .Map(path.Join(entity => entity.Alt), "alt")
            .Map(path.Join(entity => entity.TypeId), "type");
    }

    private static ClassMap<T> MapVariant<T>(this ClassMap<T> map, Expression<Func<T, CNV.Variant>> path) where T : class
    {
        var chromosomeConverter = new ChromosomeConverter();

        return map
            .Map(path.Join(entity => entity.ChromosomeId), "chromosome", chromosomeConverter)
            .Map(path.Join(entity => entity.Start), "start")
            .Map(path.Join(entity => entity.End), "end")
            .Map(path.Join(entity => entity.Length), "length")
            .Map(path.Join(entity => entity.C1Mean), "c1_mean")
            .Map(path.Join(entity => entity.C2Mean), "c2_mean")
            .Map(path.Join(entity => entity.TcnMean), "tcn_mean")
            .Map(path.Join(entity => entity.C1), "c1")
            .Map(path.Join(entity => entity.C2), "c2")
            .Map(path.Join(entity => entity.Tcn), "tcn")
            .Map(path.Join(entity => entity.TypeId), "type")
            .Map(path.Join(entity => entity.Loh), "loh")
            .Map(path.Join(entity => entity.Del), "del");
    }

    private static ClassMap<T> MapVariant<T>(this ClassMap<T> map, Expression<Func<T, SV.Variant>> path) where T : class
    {
        var chromosomeConverter = new ChromosomeConverter();

        return map
            .Map(path.Join(entity => entity.ChromosomeId), "chromosome_1", chromosomeConverter)
            .Map(path.Join(entity => entity.Start), "start_1")
            .Map(path.Join(entity => entity.End), "end_1")
            .Map(path.Join(entity => entity.OtherChromosomeId), "chromosome_2", chromosomeConverter)
            .Map(path.Join(entity => entity.OtherStart), "start_2")
            .Map(path.Join(entity => entity.OtherEnd), "end_2")
            .Map(path.Join(entity => entity.Length), "length")
            .Map(path.Join(entity => entity.TypeId), "type");
    }

    private static ClassMap<SM.VariantEntry> MapAffectedFeatures(this ClassMap<SM.VariantEntry> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SmAffectedTranscriptsConverter();
            map.Map(entity => entity.Entity.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }

        return map;
    }

    private static ClassMap<CNV.VariantEntry> MapAffectedFeatures(this ClassMap<CNV.VariantEntry> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new CnvAffectedTranscriptsConverter();
            map.Map(entity => entity.Entity.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }

        return map;
    }

    private static ClassMap<SV.VariantEntry> MapAffectedFeatures(this ClassMap<SV.VariantEntry> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SvAffectedTranscriptsConverter();
            map.Map(entity => entity.Entity.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }

        return map;
    }
}
