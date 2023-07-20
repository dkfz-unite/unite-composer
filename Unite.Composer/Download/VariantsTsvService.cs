using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Converters;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Genome;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download;

internal record VariantOccurrenceWithAffectedFeature<TVO, TV, TVAF, TF>(TVO Occurrence, TVAF AffectedFeature)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
        where TVAF : VariantAffectedFeature<TV, TF>
        where TF : Feature;

internal record SsmOccurrenceWithAffectedTranscript : VariantOccurrenceWithAffectedFeature<SSM.VariantOccurrence, SSM.Variant, SSM.AffectedTranscript, Transcript>
{
    public SsmOccurrenceWithAffectedTranscript(SSM.VariantOccurrence Occurrence, SSM.AffectedTranscript AffectedFeature) : base(Occurrence, AffectedFeature){}
}

internal record CnvOccurrenceWithAffectedTranscript : VariantOccurrenceWithAffectedFeature<CNV.VariantOccurrence, CNV.Variant, CNV.AffectedTranscript, Transcript>
{
    public CnvOccurrenceWithAffectedTranscript(CNV.VariantOccurrence Occurrence, CNV.AffectedTranscript AffectedFeature) : base(Occurrence, AffectedFeature){}
}

internal record SvOccurrenceWithAffectedTranscript : VariantOccurrenceWithAffectedFeature<SV.VariantOccurrence, SV.Variant, SV.AffectedTranscript, Transcript>
{
    public SvOccurrenceWithAffectedTranscript(SV.VariantOccurrence Occurrence, SV.AffectedTranscript AffectedFeature) : base(Occurrence, AffectedFeature){}
}

public class VariantsTsvService
{
    private readonly DomainDbContext _dbContext;    

    public VariantsTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<string> GetSsmsDataForDonors(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetSsmsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetCnvsDataForDonors(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetCnvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSvsDataForDonors(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetSvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSsmsDataForImages(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetSsmsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetCnvsDataForImages(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetCnvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSvsDataForImages(IEnumerable<int> ids, bool transcripts = false)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetSvsDataForSpecimens(specimenIds, transcripts);
    }

    public async Task<string> GetSsmsDataForSpecimens(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = _dbContext.Set<SSM.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, transcripts);
        query = IncludeVariant<SSM.VariantOccurrence, SSM.Variant>(query);
        query = IncludeSamples<SSM.VariantOccurrence, SSM.Variant>(query);
        query = OrderVariant<SSM.VariantOccurrence, SSM.Variant>(query);
        query = query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));

        var entities = await query.ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateMap<SSM.VariantOccurrence, SSM.Variant>();

        MapVariant(map);
        MapAffectedFeatures(map, transcripts);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetCnvsDataForSpecimens(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = _dbContext.Set<CNV.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, transcripts);
        query = IncludeVariant<CNV.VariantOccurrence, CNV.Variant>(query);
        query = IncludeSamples<CNV.VariantOccurrence, CNV.Variant>(query);
        query = OrderVariant<CNV.VariantOccurrence, CNV.Variant>(query);
        query = query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));

        var entities = await query.ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateMap<CNV.VariantOccurrence, CNV.Variant>();

        MapVariant(map);
        MapAffectedFeatures(map, transcripts);

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetSvsDataForSpecimens(IEnumerable<int> ids, bool transcripts = false)
    {
        var query = _dbContext.Set<SV.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, transcripts);
        query = IncludeVariant<SV.VariantOccurrence, SV.Variant>(query);
        query = IncludeSamples<SV.VariantOccurrence, SV.Variant>(query);
        query = OrderVariant<SV.VariantOccurrence, SV.Variant>(query);
        query = query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));

        var entities = await query.ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = CreateMap<SV.VariantOccurrence, SV.Variant>();

        MapVariant(map);
        MapAffectedFeatures(map, transcripts);

        return TsvWriter.Write(entities, map);
    }


    public async Task<string> GetFullSsmsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetFullSsmsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullCnvsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetFullCnvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSvsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetFullSvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSsmsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetFullSsmsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullCnvsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetFullCnvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSvsDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetFullSvsDataForSpecimens(specimenIds);
    }

    public async Task<string> GetFullSsmsDataForSpecimens(IEnumerable<int> ids)
    {
        var query = _dbContext.Set<SSM.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, true);
        query = IncludeVariant<SSM.VariantOccurrence, SSM.Variant>(query);
        query = IncludeSamples<SSM.VariantOccurrence, SSM.Variant>(query);
        query = OrderVariant<SSM.VariantOccurrence, SSM.Variant>(query);
        query = query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));

        var entities = await query.ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var entries = entities.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SsmOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        var map = CreateMap<SsmOccurrenceWithAffectedTranscript, SSM.VariantOccurrence, SSM.Variant, SSM.AffectedTranscript, Transcript>();

        MapVariant(map);
        MapAffectedFeatures(map);

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullCnvsDataForSpecimens(IEnumerable<int> ids)
    {
        var query = _dbContext.Set<CNV.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, true);
        query = IncludeVariant<CNV.VariantOccurrence, CNV.Variant>(query);
        query = IncludeSamples<CNV.VariantOccurrence, CNV.Variant>(query);
        query = OrderVariant<CNV.VariantOccurrence, CNV.Variant>(query);
        query = query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));

        var entities = await query.ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var entries = entities.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new CnvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        var map = CreateMap<CnvOccurrenceWithAffectedTranscript, CNV.VariantOccurrence, CNV.Variant, CNV.AffectedTranscript, Transcript>();

        MapVariant(map);
        MapAffectedFeatures(map);

        return TsvWriter.Write(entries, map);
    }

    public async Task<string> GetFullSvsDataForSpecimens(IEnumerable<int> ids)
    {
        var query = _dbContext.Set<SV.VariantOccurrence>().AsNoTracking();

        query = IncludeAffectedFeatures(query, true);
        query = IncludeVariant<SV.VariantOccurrence, SV.Variant>(query);
        query = IncludeSamples<SV.VariantOccurrence, SV.Variant>(query);
        query = OrderVariant<SV.VariantOccurrence, SV.Variant>(query);
        query = query.Where(entity => ids.Contains(entity.AnalysedSample.Sample.SpecimenId));

        var entities = await query.ToArrayAsync();

        var entries = entities.SelectMany(vo => vo.Variant.AffectedTranscripts, (vo, vat) => new SvOccurrenceWithAffectedTranscript(vo, vat)).ToArray();

        var map = CreateMap<SvOccurrenceWithAffectedTranscript, SV.VariantOccurrence, SV.Variant, SV.AffectedTranscript, Transcript>();

        MapVariant(map);
        MapAffectedFeatures(map);

        return TsvWriter.Write(entries, map);
    }


    private async Task<int[]> GetSpecimenIdsForDonors(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetSpecimenIdsForImages(IEnumerable<int> ids)
    { 
        var donorIds = await _dbContext.Set<Image>().AsNoTracking()
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();

        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Tissue)
            .Where(entity => entity.Tissue != null && entity.Tissue.TypeId != TissueType.Control)
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }


    private IQueryable<TVO> IncludeVariant<TVO, TV>(IQueryable<TVO> query)
        where TVO : Unite.Data.Entities.Genome.Variants.VariantOccurrence<TV>
        where TV : Unite.Data.Entities.Genome.Variants.Variant
    {
        return query
            .Include(entity => entity.Variant);
    }

    private IQueryable<TVO> IncludeSamples<TVO, TV>(IQueryable<TVO> query)
        where TVO : Unite.Data.Entities.Genome.Variants.VariantOccurrence<TV>
        where TV : Unite.Data.Entities.Genome.Variants.Variant
    {
        return query
            .Include(entity => entity.AnalysedSample.Analysis)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Donor)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Tissue)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.CellLine)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Organoid)
            .Include(entity => entity.AnalysedSample.Sample.Specimen.Xenograft)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Donor)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Tissue)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.CellLine)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Organoid)
            .Include(entity => entity.AnalysedSample.MatchedSample.Specimen.Xenograft);
    }

    private IQueryable<SSM.VariantOccurrence> IncludeAffectedFeatures(IQueryable<SSM.VariantOccurrence> query, bool transcripts = false)
    {
        IQueryable<SSM.VariantOccurrence> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private IQueryable<CNV.VariantOccurrence> IncludeAffectedFeatures(IQueryable<CNV.VariantOccurrence> query, bool transcripts = false)
    {
        IQueryable<CNV.VariantOccurrence> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private IQueryable<SV.VariantOccurrence> IncludeAffectedFeatures(IQueryable<SV.VariantOccurrence> query, bool transcripts = false)
    {
        IQueryable<SV.VariantOccurrence> includeQuery = query;

        if (transcripts)
        {
            includeQuery = includeQuery
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Gene)
                .Include(entity => entity.Variant.AffectedTranscripts)
                    .ThenInclude(entity => entity.Feature.Protein);
        }
        
        return includeQuery;
    }

    private IQueryable<TVO> OrderVariant<TVO, TV>(IQueryable<TVO> query)
        where TVO : Unite.Data.Entities.Genome.Variants.VariantOccurrence<TV>
        where TV : Unite.Data.Entities.Genome.Variants.Variant
    {
        return query
            .OrderBy(entity => entity.AnalysedSample.Sample.Specimen.DonorId)
            .ThenBy(entity => entity.Variant.ChromosomeId)
            .ThenBy(entity => entity.Variant.Start);
    }

    private IQueryable<TVO> FilterByDonors<TVO, TV>(IQueryable<TVO> query, IEnumerable<int> ids)
        where TVO : Unite.Data.Entities.Genome.Variants.VariantOccurrence<TV>
        where TV : Unite.Data.Entities.Genome.Variants.Variant
    {
        return query
            .Where(entity => ids.Contains(entity.AnalysedSample.Sample.Specimen.Donor.Id));
    }


    private static ClassMap<TVO> CreateMap<TVO, TV>()
        where TVO : Unite.Data.Entities.Genome.Variants.VariantOccurrence<TV>
        where TV : Unite.Data.Entities.Genome.Variants.Variant
    {
        return new ClassMap<TVO>()
            .Map(entity => entity.AnalysedSample.Sample.Specimen.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.AnalysedSample.Sample.Specimen.ReferenceId, "specimen_id")
            .Map(entity => entity.AnalysedSample.Sample.Specimen.Type, "specimen_type")
            .Map(entity => entity.AnalysedSample.Sample.ReferenceId, "sample_id")
            .Map(entity => entity.AnalysedSample.MatchedSample.Specimen.ReferenceId, "matched_specimen_id")
            .Map(entity => entity.AnalysedSample.MatchedSample.Specimen.Type, "matched_specimen_type")
            .Map(entity => entity.AnalysedSample.MatchedSample.ReferenceId, "matched_sample_id")
            .Map(entity => entity.AnalysedSample.Analysis.TypeId, "analysis_type")
            .Map(entity => entity.Variant.Id, "variant_id");
    }

    private static ClassMap<T> CreateMap<T, TVO, TV, TVAF, TF>()
        where T : VariantOccurrenceWithAffectedFeature<TVO, TV, TVAF, TF>
        where TVO : VariantOccurrence<TV>
        where TV : Variant
        where TVAF : VariantAffectedFeature<TV, TF>
        where TF : Feature
    {
        return new ClassMap<T>()
            .Map(entry => entry.Occurrence.AnalysedSample.Sample.Specimen.Donor.ReferenceId, "donor_id")
            .Map(entry => entry.Occurrence.AnalysedSample.Sample.Specimen.ReferenceId, "specimen_id")
            .Map(entry => entry.Occurrence.AnalysedSample.Sample.Specimen.Type, "specimen_type")
            .Map(entry => entry.Occurrence.AnalysedSample.Sample.ReferenceId, "sample_id")
            .Map(entry => entry.Occurrence.AnalysedSample.MatchedSample.Specimen.ReferenceId, "matched_specimen_id")
            .Map(entry => entry.Occurrence.AnalysedSample.MatchedSample.Specimen.Type, "matched_specimen_type")
            .Map(entry => entry.Occurrence.AnalysedSample.MatchedSample.ReferenceId, "matched_sample_id")
            .Map(entry => entry.Occurrence.AnalysedSample.Analysis.TypeId, "analysis_type")
            .Map(entry => entry.Occurrence.Variant.Id, "variant_id");
    }


    private static void MapVariant(ClassMap<SSM.VariantOccurrence> map)
    {
        var chromosomeConverter = new ChromosomeConverter();
        
        map.Map(entity => entity.Variant.ChromosomeId, "chromosome", chromosomeConverter)
           .Map(entity => entity.Variant.Start, "start")
           .Map(entity => entity.Variant.End, "end")
           .Map(entity => entity.Variant.Length, "length")
           .Map(entity => entity.Variant.Ref, "ref")
           .Map(entity => entity.Variant.Alt, "alt")
           .Map(entity => entity.Variant.TypeId, "type");
    }

    private static void MapVariant(ClassMap<CNV.VariantOccurrence> map)
    {
        var chromosomeConverter = new ChromosomeConverter();

        map.Map(entity => entity.Variant.ChromosomeId, "chromosome", chromosomeConverter)
           .Map(entity => entity.Variant.Start, "start")
           .Map(entity => entity.Variant.End, "end")
           .Map(entity => entity.Variant.Length, "length")
           .Map(entity => entity.Variant.C1Mean, "c1_mean")
           .Map(entity => entity.Variant.C2Mean, "c2_mean")
           .Map(entity => entity.Variant.TcnMean, "tcn_mean")
           .Map(entity => entity.Variant.C1, "c1")
           .Map(entity => entity.Variant.C2, "c2")
           .Map(entity => entity.Variant.Tcn, "tcn")
           .Map(entity => entity.Variant.TypeId, "type")
           .Map(entity => entity.Variant.Loh, "loh")
           .Map(entity => entity.Variant.HomoDel, "homo_del");
    }

    private static void MapVariant(ClassMap<SV.VariantOccurrence> map)
    {
        var chromosomeConverter = new ChromosomeConverter();

        map.Map(entity => entity.Variant.ChromosomeId, "chromosome_1", chromosomeConverter)
           .Map(entity => entity.Variant.Start, "start_1")
           .Map(entity => entity.Variant.End, "end_1")
           .Map(entity => entity.Variant.OtherChromosomeId, "chromosome_2", chromosomeConverter)
           .Map(entity => entity.Variant.OtherStart, "start_2")
           .Map(entity => entity.Variant.OtherEnd, "end_2")
           .Map(entity => entity.Variant.Length, "length")
           .Map(entity => entity.Variant.TypeId, "type");
    }


    private static void MapVariant(ClassMap<SsmOccurrenceWithAffectedTranscript> map)
    {
        var chromosomeConverter = new ChromosomeConverter();

        map.Map(entity => entity.Occurrence.Variant.ChromosomeId, "chromosome", chromosomeConverter)
           .Map(entity => entity.Occurrence.Variant.Start, "start")
           .Map(entity => entity.Occurrence.Variant.End, "end")
           .Map(entity => entity.Occurrence.Variant.Length, "length")
           .Map(entity => entity.Occurrence.Variant.Ref, "ref")
           .Map(entity => entity.Occurrence.Variant.Alt, "alt")
           .Map(entity => entity.Occurrence.Variant.TypeId, "type");
    }

    private static void MapVariant(ClassMap<CnvOccurrenceWithAffectedTranscript> map)
    {
        var chromosomeConverter = new ChromosomeConverter();

        map.Map(entity => entity.Occurrence.Variant.ChromosomeId, "chromosome", chromosomeConverter)
           .Map(entity => entity.Occurrence.Variant.Start, "start")
           .Map(entity => entity.Occurrence.Variant.End, "end")
           .Map(entity => entity.Occurrence.Variant.Length, "length")
           .Map(entity => entity.Occurrence.Variant.C1Mean, "c1_mean")
           .Map(entity => entity.Occurrence.Variant.C2Mean, "c2_mean")
           .Map(entity => entity.Occurrence.Variant.TcnMean, "tcn_mean")
           .Map(entity => entity.Occurrence.Variant.C1, "c1")
           .Map(entity => entity.Occurrence.Variant.C2, "c2")
           .Map(entity => entity.Occurrence.Variant.Tcn, "tcn")
           .Map(entity => entity.Occurrence.Variant.TypeId, "type")
           .Map(entity => entity.Occurrence.Variant.Loh, "loh")
           .Map(entity => entity.Occurrence.Variant.HomoDel, "homo_del");
    }

    private static void MapVariant(ClassMap<SvOccurrenceWithAffectedTranscript> map)
    {
        var chromosomeConverter = new ChromosomeConverter();

        map.Map(entity => entity.Occurrence.Variant.ChromosomeId, "chromosome_1", chromosomeConverter)
           .Map(entity => entity.Occurrence.Variant.Start, "start_1")
           .Map(entity => entity.Occurrence.Variant.End, "end_1")
           .Map(entity => entity.Occurrence.Variant.OtherChromosomeId, "chromosome_2", chromosomeConverter)
           .Map(entity => entity.Occurrence.Variant.OtherStart, "start_2")
           .Map(entity => entity.Occurrence.Variant.OtherEnd, "end_2")
           .Map(entity => entity.Occurrence.Variant.Length, "length")
           .Map(entity => entity.Occurrence.Variant.TypeId, "type");
    }


    private static void MapAffectedFeatures(ClassMap<SSM.VariantOccurrence> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SsmAffectedTranscriptsConverter();
            map.Map(entity => entity.Variant.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }
    }

    private static void MapAffectedFeatures(ClassMap<CNV.VariantOccurrence> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new CnvAffectedTranscriptsConverter();
            map.Map(entity => entity.Variant.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }
    }

    private static void MapAffectedFeatures(ClassMap<SV.VariantOccurrence> map, bool transcripts = false)
    {
        if (transcripts)
        {
            var affectedTranscriptsConverter = new SvAffectedTranscriptsConverter();
            map.Map(entity => entity.Variant.AffectedTranscripts, "affected_genes", affectedTranscriptsConverter);
        }
    }


    private static void MapAffectedFeatures(ClassMap<SsmOccurrenceWithAffectedTranscript> map)
    {
        var codonChangeConverter = new CodonChangeConverter();
        var proteinChangeConverter = new ProteinChangeConverter();
        var consequencesConverter = new ConsequencesConverter();

        map.Map(entry => entry.AffectedFeature.Feature.Gene.StableId, "gene_id")
           .Map(entry => entry.AffectedFeature.Feature.Gene.Symbol, "gene_symbol")
           .Map(entry => entry.AffectedFeature.Feature.StableId, "transcript_id")
           .Map(entry => entry.AffectedFeature.Feature.Symbol, "transcript_symbol")
           .Map(entry => entry.AffectedFeature.Feature.Protein.StableId, "protein_id")
           .Map(entry => entry.AffectedFeature.Distance, "distance")
           .Map(entry => entry.AffectedFeature.CodonChange, "cdna_change", codonChangeConverter)
           .Map(entry => entry.AffectedFeature.AminoAcidChange, "aa_change", proteinChangeConverter)
           .Map(entry => entry.AffectedFeature.Consequences, "consequences", consequencesConverter);
    }

    private static void MapAffectedFeatures(ClassMap<CnvOccurrenceWithAffectedTranscript> map)
    {
        var consequencesConverter = new ConsequencesConverter();

        map.Map(entry => entry.AffectedFeature.Feature.Gene.StableId, "gene_id")
           .Map(entry => entry.AffectedFeature.Feature.Gene.Symbol, "gene_symbol")
           .Map(entry => entry.AffectedFeature.Feature.StableId, "transcript_id")
           .Map(entry => entry.AffectedFeature.Feature.Symbol, "transcript_symbol")
           .Map(entry => entry.AffectedFeature.Feature.Protein.StableId, "protein_id")
           .Map(entry => entry.AffectedFeature.Distance, "distance")
           .Map(entry => entry.AffectedFeature.OverlapBpNumber, "overlap_bp_number")
           .Map(entry => entry.AffectedFeature.OverlapPercentage, "overlap_percentage")
           .Map(entry => entry.AffectedFeature.CDNAStart, "cdna_breaking_point")
           .Map(entry => entry.AffectedFeature.CDSStart, "cds_breaking_point")
           .Map(entry => entry.AffectedFeature.ProteinStart, "protein_breaking_point")
           .Map(entry => entry.AffectedFeature.Consequences, "consequences", consequencesConverter);
    }

    private static void MapAffectedFeatures(ClassMap<SvOccurrenceWithAffectedTranscript> map)
    {
        var consequencesConverter = new ConsequencesConverter();

        map.Map(entry => entry.AffectedFeature.Feature.Gene.StableId, "gene_id")
           .Map(entry => entry.AffectedFeature.Feature.Gene.Symbol, "gene_symbol")
           .Map(entry => entry.AffectedFeature.Feature.StableId, "transcript_id")
           .Map(entry => entry.AffectedFeature.Feature.Symbol, "transcript_symbol")
           .Map(entry => entry.AffectedFeature.Feature.Protein.StableId, "protein_id")
           .Map(entry => entry.AffectedFeature.Distance, "distance")
           .Map(entry => entry.AffectedFeature.OverlapBpNumber, "overlap_bp_number")
           .Map(entry => entry.AffectedFeature.OverlapPercentage, "overlap_percentage")
           .Map(entry => entry.AffectedFeature.CDNAStart, "cdna_breaking_point")
           .Map(entry => entry.AffectedFeature.CDSStart, "cds_breaking_point")
           .Map(entry => entry.AffectedFeature.ProteinStart, "protein_breaking_point")
           .Map(entry => entry.AffectedFeature.Consequences, "consequences", consequencesConverter);
    }
}
