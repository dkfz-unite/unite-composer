using System.Linq.Expressions;
using Unite.Composer.Download.Tsv.Mapping.Converters;
using Unite.Composer.Download.Extensions;
using Unite.Composer.Download.Tsv.Mapping.Models;
using Unite.Data.Entities.Genome.Analysis;
using Unite.Essentials.Tsv;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv.Mapping.Extensions;

internal static class VariantsMappingExtensions
{
    internal static ClassMap<SSM.VariantOccurrence> MapVariantOccurrences(this ClassMap<SSM.VariantOccurrence> map, bool transcripts = false)
    {
        return map.MapAnalysedSample(entity => entity.AnalysedSample)
                  .MapVariant(entity => entity.Variant)
                  .MapAffectedFeatures(transcripts);
    }

    internal static ClassMap<CNV.VariantOccurrence> MapVariantOccurrences(this ClassMap<CNV.VariantOccurrence> map, bool transcripts = false)
    {
        return map.MapAnalysedSample(entity => entity.AnalysedSample)
                  .MapVariant(entity => entity.Variant)
                  .MapAffectedFeatures(transcripts);
    }

    internal static ClassMap<SV.VariantOccurrence> MapVariantOccurrences(this ClassMap<SV.VariantOccurrence> map, bool transcripts = false)
    {
        return map.MapAnalysedSample(entity => entity.AnalysedSample)
                  .MapVariant(entity => entity.Variant)
                  .MapAffectedFeatures(transcripts);
    }


    internal static ClassMap<SsmOccurrenceWithAffectedTranscript> MapVariantOccurrences(this ClassMap<SsmOccurrenceWithAffectedTranscript> map)
    {
        return map.MapAnalysedSample(entry => entry.Occurrence.AnalysedSample)
                  .MapVariant(entry => entry.Occurrence.Variant)
                  .MapAffectedTranscript(entry => entry.AffectedFeature);
    }

    internal static ClassMap<CnvOccurrenceWithAffectedTranscript> MapVariantOccurrences(this ClassMap<CnvOccurrenceWithAffectedTranscript> map)
    {
        return map.MapAnalysedSample(entry => entry.Occurrence.AnalysedSample)
                  .MapVariant(entry => entry.Occurrence.Variant)
                  .MapAffectedTranscript(entr => entr.AffectedFeature);
    }

    internal static ClassMap<SvOccurrenceWithAffectedTranscript> MapVariantOccurrences(this ClassMap<SvOccurrenceWithAffectedTranscript> map)
    {
        return map.MapAnalysedSample(entry => entry.Occurrence.AnalysedSample)
                  .MapVariant(entry => entry.Occurrence.Variant)
                  .MapAffectedTranscript(entr => entr.AffectedFeature);
    }


    private static ClassMap<T> MapAnalysedSample<T>(this ClassMap<T> map, Expression<Func<T, AnalysedSample>> path) where T : class
    {
        return map
            .Map(path.Join(entity => entity.Sample.Specimen.Donor.ReferenceId), "donor_id")
            .Map(path.Join(entity => entity.Sample.Specimen.ReferenceId), "specimen_id")
            .Map(path.Join(entity => entity.Sample.Specimen.Type), "specimen_type")
            .Map(path.Join(entity => entity.Sample.ReferenceId), "sample_id")
            .Map(path.Join(entity => entity.MatchedSample.Specimen.ReferenceId), "matched_specimen_id")
            .Map(path.Join(entity => entity.MatchedSample.Specimen.Type), "matched_specimen_type")
            .Map(path.Join(entity => entity.MatchedSample.ReferenceId), "matched_sample_id")
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
            .Map(path.Join(entity => entity.HomoDel), "homo_del");
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


    private static ClassMap<SSM.VariantOccurrence> MapAffectedFeatures(this ClassMap<SSM.VariantOccurrence> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SsmAffectedTranscriptsConverter();
            map.Map(entity => entity.Variant.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }

        return map;
    }

    private static ClassMap<CNV.VariantOccurrence> MapAffectedFeatures(this ClassMap<CNV.VariantOccurrence> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new CnvAffectedTranscriptsConverter();
            map.Map(entity => entity.Variant.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }

        return map;
    }

    private static ClassMap<SV.VariantOccurrence> MapAffectedFeatures(this ClassMap<SV.VariantOccurrence> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SvAffectedTranscriptsConverter();
            map.Map(entity => entity.Variant.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }

        return map;
    }


    private static ClassMap<T> MapAffectedTranscript<T>(this ClassMap<T> map, Expression<Func<T, SSM.AffectedTranscript>> path) where T : class
    {
        var codonChangeConverter = new CodonChangeConverter();
        var proteinChangeConverter = new ProteinChangeConverter();
        var consequencesConverter = new ConsequencesConverter();

        return map
            .Map(path.Join(entity => entity.Feature.Gene.StableId), "gene_id")
            .Map(path.Join(entity => entity.Feature.Gene.Symbol), "gene_symbol")
            .Map(path.Join(entity => entity.Feature.StableId), "transcript_id")
            .Map(path.Join(entity => entity.Feature.Symbol), "transcript_symbol")
            .Map(path.Join(entity => entity.Feature.Protein.StableId), "protein_id")
            .Map(path.Join(entity => entity.Distance), "distance")
            .Map(path.Join(entity => entity.CodonChange), "cdna_change", codonChangeConverter)
            .Map(path.Join(entity => entity.AminoAcidChange), "aa_change", proteinChangeConverter)
            .Map(path.Join(entity => entity.Consequences), "consequences", consequencesConverter);
    }

    private static ClassMap<T> MapAffectedTranscript<T>(this ClassMap<T> map, Expression<Func<T, CNV.AffectedTranscript>> path) where T : class
    {
        var consequencesConverter = new ConsequencesConverter();

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
            .Map(path.Join(entity => entity.Consequences), "consequences", consequencesConverter);
    }

    private static ClassMap<T> MapAffectedTranscript<T>(this ClassMap<T> map, Expression<Func<T, SV.AffectedTranscript>> path) where T : class
    {
        var consequencesConverter = new ConsequencesConverter();

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
            .Map(path.Join(entity => entity.Consequences), "consequences", consequencesConverter);
    }
}
