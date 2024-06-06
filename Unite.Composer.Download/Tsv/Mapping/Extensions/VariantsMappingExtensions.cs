using System.Linq.Expressions;
using Unite.Composer.Download.Tsv.Mapping.Converters;
using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Entities.Genome.Analysis;
using Unite.Data.Entities.Genome.Analysis.Dna;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

using SSM = Unite.Data.Entities.Genome.Analysis.Dna.Ssm;
using CNV = Unite.Data.Entities.Genome.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Genome.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class VariantsMappingExtensions
{
    public static ClassMap<TVE> MapVariantEntries<TVE, TV>(this ClassMap<TVE> map, bool transcripts = false)
        where TVE : VariantEntry<TV>
        where TV : Variant
    {
        if (map is ClassMap<SSM.VariantEntry> ssmMap)
            ssmMap.MapVariantEntries(transcripts);
        else if (map is ClassMap<CNV.VariantEntry> cnvMap)
            cnvMap.MapVariantEntries(transcripts);
        else if (map is ClassMap<SV.VariantEntry> svMap)
            svMap.MapVariantEntries(transcripts);

        return map;
    }

    public static ClassMap<SSM.VariantEntry> MapVariantEntries(this ClassMap<SSM.VariantEntry> map, bool transcripts = false)
    {
        return map.MapSample(entity => entity.Sample)
                  .MapVariant(entity => entity.Entity)
                  .MapAffectedFeatures(transcripts);
    }

    public static ClassMap<CNV.VariantEntry> MapVariantEntries(this ClassMap<CNV.VariantEntry> map, bool transcripts = false)
    {
        return map.MapSample(entity => entity.Sample)
                  .MapVariant(entity => entity.Entity)
                  .MapAffectedFeatures(transcripts);
    }

    public static ClassMap<SV.VariantEntry> MapVariantEntries(this ClassMap<SV.VariantEntry> map, bool transcripts = false)
    {
        return map.MapSample(entity => entity.Sample)
                  .MapVariant(entity => entity.Entity)
                  .MapAffectedFeatures(transcripts);
    }


    public static ClassMap<SsmEntryWithAffectedTranscript> MapVariantEntries(this ClassMap<SsmEntryWithAffectedTranscript> map)
    {
        return map.MapSample(entry => entry.Entry.Sample)
                  .MapVariant(entry => entry.Entry.Entity)
                  .MapAffectedTranscript(entry => entry.AffectedFeature);
    }

    public static ClassMap<CnvEntryWithAffectedTranscript> MapVariantEntries(this ClassMap<CnvEntryWithAffectedTranscript> map)
    {
        return map.MapSample(entry => entry.Entry.Sample)
                  .MapVariant(entry => entry.Entry.Entity)
                  .MapAffectedTranscript(entry => entry.AffectedFeature);
    }

    public static ClassMap<SvEntryWithAffectedTranscript> MapVariantEntries(this ClassMap<SvEntryWithAffectedTranscript> map)
    {
        return map.MapSample(entry => entry.Entry.Sample)
                  .MapVariant(entry => entry.Entry.Entity)
                  .MapAffectedTranscript(entry => entry.AffectedFeature);
    }


    private static ClassMap<T> MapSample<T>(this ClassMap<T> map, Expression<Func<T, Sample>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.Specimen.Donor.ReferenceId), "donor_id")
            .Map(path.Join(entity => entity.Specimen.ReferenceId), "specimen_id")
            .Map(path.Join(entity => entity.Specimen.TypeId), "specimen_type")
            .Map(path.Join(entity => entity.MatchedSample.Specimen.ReferenceId), "matched_specimen_id")
            .Map(path.Join(entity => entity.MatchedSample.Specimen.TypeId), "matched_specimen_type")
            .Map(path.Join(entity => entity.Analysis.TypeId), "analysis_type");
    }


    private static ClassMap<T> MapVariant<T>(this ClassMap<T> map, Expression<Func<T, SSM.Variant>> path) where T : class
    {
        var chromosomeConverter = new ChromosomeConverter();

        return map
            .Map(path.Join(entity => entity.Id), "variant_id")
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
            .Map(path.Join(entity => entity.Id), "variant_id")
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
            .Map(path.Join(entity => entity.Id), "variant_id")
            .Map(path.Join(entity => entity.ChromosomeId), "chromosome_1", chromosomeConverter)
            .Map(path.Join(entity => entity.Start), "start_1")
            .Map(path.Join(entity => entity.End), "end_1")
            .Map(path.Join(entity => entity.OtherChromosomeId), "chromosome_2", chromosomeConverter)
            .Map(path.Join(entity => entity.OtherStart), "start_2")
            .Map(path.Join(entity => entity.OtherEnd), "end_2")
            .Map(path.Join(entity => entity.Length), "length")
            .Map(path.Join(entity => entity.TypeId), "type");
    }


    private static ClassMap<SSM.VariantEntry> MapAffectedFeatures(this ClassMap<SSM.VariantEntry> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SsmAffectedTranscriptsConverter();
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


    private static ClassMap<T> MapAffectedTranscript<T>(this ClassMap<T> map, Expression<Func<T, SSM.AffectedTranscript>> path) where T : class
    {
        var codonChangeConverter = new CodonChangeConverter();
        var proteinChangeConverter = new ProteinChangeConverter();
        var effectsConverter = new EffectsConverter();

        return map
            .Map(path.Join(entity => entity.Feature.Gene.StableId), "gene_id")
            .Map(path.Join(entity => entity.Feature.Gene.Symbol), "gene_symbol")
            .Map(path.Join(entity => entity.Feature.StableId), "transcript_id")
            .Map(path.Join(entity => entity.Feature.Symbol), "transcript_symbol")
            .Map(path.Join(entity => entity.Feature.Protein.StableId), "protein_id")
            .Map(path.Join(entity => entity.Distance), "distance")
            .Map(path.Join(entity => entity.CodonChange), "codon_change", codonChangeConverter)
            .Map(path.Join(entity => entity.AminoAcidChange), "protein_change", proteinChangeConverter)
            .Map(path.Join(entity => entity.Effects), "effects", effectsConverter);
    }

    private static ClassMap<T> MapAffectedTranscript<T>(this ClassMap<T> map, Expression<Func<T, CNV.AffectedTranscript>> path) where T : class
    {
        var effectsConverter = new EffectsConverter();

        return map
            .Map(path.Join(entity => entity.Feature.Gene.StableId), "gene_id")
            .Map(path.Join(entity => entity.Feature.Gene.Symbol), "gene_symbol")
            .Map(path.Join(entity => entity.Feature.StableId), "transcript_id")
            .Map(path.Join(entity => entity.Feature.Symbol), "transcript_symbol")
            .Map(path.Join(entity => entity.Feature.Protein.StableId), "protein_id")
            .Map(path.Join(entity => entity.Distance), "distance")
            .Map(path.Join(entity => entity.OverlapBpNumber), "overlap_bp_number")
            .Map(path.Join(entity => entity.OverlapPercentage), "overlap_percentage")
            .Map(path.Join(entity => entity.CDNAStart), "cdna_breaking_point")
            .Map(path.Join(entity => entity.CDSStart), "cds_breaking_point")
            .Map(path.Join(entity => entity.ProteinStart), "protein_breaking_point")
            .Map(path.Join(entity => entity.Effects), "effects", effectsConverter);
    }

    private static ClassMap<T> MapAffectedTranscript<T>(this ClassMap<T> map, Expression<Func<T, SV.AffectedTranscript>> path) where T : class
    {
        var effectsConverter = new EffectsConverter();

        return map
            .Map(path.Join(entity => entity.Feature.Gene.StableId), "gene_id")
            .Map(path.Join(entity => entity.Feature.Gene.Symbol), "gene_symbol")
            .Map(path.Join(entity => entity.Feature.StableId), "transcript_id")
            .Map(path.Join(entity => entity.Feature.Symbol), "transcript_symbol")
            .Map(path.Join(entity => entity.Feature.Protein.StableId), "protein_id")
            .Map(path.Join(entity => entity.Distance), "distance")
            .Map(path.Join(entity => entity.OverlapBpNumber), "overlap_bp_number")
            .Map(path.Join(entity => entity.OverlapPercentage), "overlap_percentage")
            .Map(path.Join(entity => entity.CDNAStart), "cdna_breaking_point")
            .Map(path.Join(entity => entity.CDSStart), "cds_breaking_point")
            .Map(path.Join(entity => entity.ProteinStart), "protein_breaking_point")
            .Map(path.Join(entity => entity.Effects), "effects", effectsConverter);
    }
}
