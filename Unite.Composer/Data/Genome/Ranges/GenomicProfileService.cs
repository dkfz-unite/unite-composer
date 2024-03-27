using Microsoft.EntityFrameworkCore;
using Unite.Composer.Data.Genome.Ranges.Models;
using Unite.Data.Context;
using Unite.Data.Entities.Genome;
using Unite.Data.Entities.Genome.Enums;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Essentials.Extensions;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Data.Genome.Ranges;

public class GenomicProfileService
{
    private readonly GenomicRangesFilterService _rangesService;
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;

    public GenomicProfileService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _rangesService = new GenomicRangesFilterService();
        _dbContextFactory = dbContextFactory;
    }

    public async Task<GenomicRangesData> GetProfile(int specimenId, GenomicRangesFilterCriteria filterCriteria)
    {
        
        var ranges = _rangesService.GetRanges(filterCriteria).ToArray();
        
        var startChr = ranges.Min(range => range.Chr);
        var start = ranges.Where(range => range.Chr == startChr).Min(range => range.Start);
        var endChr = ranges.Max(range => range.Chr);
        var end = ranges.Where(range => range.Chr == endChr).Max(range => range.End);
        var index = 0;

        ranges.ForEach(range => range.Index = index++);

        var profile = new GenomicRangesData(ranges)
        {
            HasSsms = HasSsms(specimenId),
            HasCnvs = HasCnvs(specimenId),
            HasSvs = HasSvs(specimenId),
            HasExps = HasExpressions(specimenId)
        };

        await Task.WhenAll(
            LoadGenes(startChr, start, endChr, end, ranges[0].Length).ContinueWith(task => profile.Genes = GetGenesData(task.Result, ref ranges)),
            LoadSsms(specimenId, startChr, start, endChr, end).ContinueWith(task => profile.Ssms = GetSsmsData(task.Result, ref ranges)),
            LoadCnvs(specimenId, startChr, start, endChr, end).ContinueWith(task => profile.Cnvs = GetCnvsData(task.Result, ref ranges)),
            LoadSvs(specimenId, startChr, start, endChr, end).ContinueWith(task => profile.Svs = GetSvsData(task.Result, ref ranges)),
            LoadExpressions(specimenId, startChr, start, endChr, end).ContinueWith(task => profile.Exps = GetExpressionsData(task.Result, ref ranges))
        );

        return profile;
    }


    private static Models.Profile.GenesData[] GetGenesData(in Gene[] genes, ref GenomicRange[] ranges)
    {
        var data = new Dictionary<int, Models.Profile.GenesData>();
        
        foreach (var range in ranges)
        {
            var rangeGenes = genes.Where(gene =>
                gene.ChromosomeId == (Chromosome)range.Chr &&
                ((gene.End >= range.Start && gene.End <= range.End) ||
                (gene.Start >= range.Start && gene.Start <= range.End) ||
                (gene.Start >= range.Start && gene.End <= range.End) ||
                (gene.Start <= range.Start && gene.End >= range.End))
            );

            var gene = rangeGenes.FirstOrDefault();

            if (gene != null)
            {
                var exists = data.TryGetValue(gene.Id, out var geneData);
                
                if (!exists)
                {
                    geneData = new Models.Profile.GenesData([range.Index, range.Index], gene);

                    data.Add(gene.Id, geneData);
                }
                else
                {
                    geneData.Range[1] = range.Index;
                }
            }
        }
    
        return data.Values.ToArrayOrNull();
    }

    private static Models.Profile.SsmsData[] GetSsmsData(in SSM.Variant[] variants, ref GenomicRange[] ranges)
    {
        var data = new List<Models.Profile.SsmsData>();
        
        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.End >= range.Start && variant.End <= range.End) ||
                (variant.Start >= range.Start && variant.Start <= range.End) ||
                (variant.Start <= range.Start && variant.End >= range.End))
            ).ToArray();

            if (rangeVariants.Length > 1)
                data.Add(new Models.Profile.SsmsData([range.Index, range.Index], rangeVariants));
            else if (rangeVariants.Length == 1)
                data.Add(new Models.Profile.SsmsData([range.Index, range.Index], rangeVariants[0]));
        }

        return data.ToArrayOrNull();
    }

    public static Models.Profile.CnvsData[] GetCnvsData(in CNV.Variant[] variants, ref GenomicRange[] ranges)
    {
        var data = new Dictionary<long, Models.Profile.CnvsData>();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.End >= range.Start && variant.End <= range.End) ||
                (variant.Start >= range.Start && variant.Start <= range.End) ||
                (variant.Start <= range.Start && variant.End >= range.End))
            );

            var variant = rangeVariants.FirstOrDefault(variant => variant.TypeId == CNV.Enums.CnvType.Loss) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == CNV.Enums.CnvType.Gain) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == CNV.Enums.CnvType.Neutral) ??
                          rangeVariants.FirstOrDefault();

            if (variant != null)
            {
                var exists = data.TryGetValue(variant.Id, out var cnvData);

                if (!exists)
                {
                    cnvData = new Models.Profile.CnvsData([range.Index, range.Index], variant);

                    data.Add(variant.Id, cnvData);
                }
                else
                {
                    cnvData.Range[1] = range.Index;
                }
            }
        }

        return data.Values.ToArrayOrNull();
    }

    private static Models.Profile.SvsData[] GetSvsData(in SV.Variant[] variants, ref GenomicRange[] ranges)
    {
        var data = new Dictionary<long, Models.Profile.SvsData>();

        foreach (var range in ranges)
        {
            var rangeVariants = variants.Where(variant =>
                variant.ChromosomeId == (Chromosome)range.Chr &&
                ((variant.End >= range.Start && variant.End <= range.End) ||
                (variant.OtherStart >= range.Start && variant.OtherStart <= range.End) ||
                (variant.End <= range.Start && variant.OtherStart >= range.End))
            );

            var variant = rangeVariants.FirstOrDefault(variant => variant.TypeId == SV.Enums.SvType.ITX) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == SV.Enums.SvType.CTX) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == SV.Enums.SvType.DEL) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == SV.Enums.SvType.DUP) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == SV.Enums.SvType.TDUP) ??
                          rangeVariants.FirstOrDefault(variant => variant.TypeId == SV.Enums.SvType.INS) ??
                          rangeVariants.FirstOrDefault();

            if (variant != null)
            {
                var exists = data.TryGetValue(variant.Id, out var svData);

                if (!exists)
                {
                    svData = new Models.Profile.SvsData([range.Index, range.Index], variant);

                    data.Add(variant.Id, svData);
                }
                else
                {
                    if (variant.TypeId != SV.Enums.SvType.ITX && variant.TypeId != SV.Enums.SvType.CTX)
                        svData.Range[1] = range.Index;
                }
            }
        }

        return data.Values.ToArrayOrNull();
    }

    private static Models.Profile.ExpressionData[] GetExpressionsData(in BulkExpression[] expressions, ref GenomicRange[] ranges)
    {
        var data = new List<Models.Profile.ExpressionData>();

        foreach (var range in ranges)
        {
            var rangeExpressions = expressions.Where(expression =>
                expression.Entity.ChromosomeId == (Chromosome)range.Chr &&
                ((expression.Entity.End >= range.Start && expression.Entity.End <= range.End) ||
                (expression.Entity.Start >= range.Start && expression.Entity.Start <= range.End) ||
                (expression.Entity.Start >= range.Start && expression.Entity.End <= range.End) ||
                (expression.Entity.Start <= range.Start && expression.Entity.End >= range.End))
            ).ToArray();

            if (rangeExpressions.Length > 1)
                data.Add(new Models.Profile.ExpressionData([range.Index, range.Index], rangeExpressions));
            else if (rangeExpressions.Length == 1)
                data.Add(new Models.Profile.ExpressionData([range.Index, range.Index], rangeExpressions[0]));
        }

        return data.ToArrayOrNull();
    }

    private async Task<Gene[]> LoadGenes(int startChr, int start, int endChr, int end, int length)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<Gene>()
            .AsNoTracking()
            .Where(gene => (int)gene.ChromosomeId >= startChr && (int)gene.ChromosomeId <= endChr)
            .Where(gene => gene.End - gene.Start + 1 >= length)
            .ToArrayAsync();
    }

    private async Task<SSM.Variant[]> LoadSsms(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<SSM.VariantEntry>().AsNoTracking()
            .Include(entry => entry.Entity.AffectedTranscripts)
                .ThenInclude(transcript => transcript.Feature)
            .Where(entry => entry.AnalysedSample.TargetSampleId == specimenId)
            .Where(entry => entry.Entity.AffectedTranscripts.Any())
            .Where(entry => (int)entry.Entity.ChromosomeId >= startChr && (int)entry.Entity.ChromosomeId <= endChr)
            .Select(entry => entry.Entity)
            .ToArrayAsync();
    }

    private async Task<CNV.Variant[]> LoadCnvs(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<CNV.VariantEntry>()
            .AsNoTracking()
            .Include(entry => entry.Entity.AffectedTranscripts)
                .ThenInclude(transcript => transcript.Feature)
            .Where(entry => entry.AnalysedSample.TargetSampleId == specimenId)
            .Where(entry => (int)entry.Entity.ChromosomeId >= startChr && (int)entry.Entity.ChromosomeId <= endChr)
            .Select(entry => entry.Entity)
            .ToArrayAsync();
    }

    private async Task<SV.Variant[]> LoadSvs(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<SV.VariantEntry>()
            .AsNoTracking()
            .Include(entry => entry.Entity.AffectedTranscripts)
                .ThenInclude(transcript => transcript.Feature)
            .Where(entry => entry.AnalysedSample.TargetSampleId == specimenId)
            // .Where(entry => entry.Entity.TypeId != SV.Enums.SvType.ITX && entry.Entity.TypeId != SV.Enums.SvType.CTX)
            .Where(entry => ((int)entry.Entity.ChromosomeId >= startChr && (int)entry.Entity.ChromosomeId <= endChr) ||
                           ((int)entry.Entity.OtherChromosomeId >= startChr && (int)entry.Entity.OtherChromosomeId <= endChr))
            .Select(entry => entry.Entity)
            .ToArrayAsync();
    }

    private async Task<BulkExpression[]> LoadExpressions(int specimenId, int startChr, int start, int endChr, int end)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await dbContext.Set<BulkExpression>()
            .AsNoTracking()
            .Include(expression => expression.Entity)
            .Where(expression => expression.AnalysedSample.TargetSampleId == specimenId)
            .Where(expression => (int)expression.Entity.ChromosomeId >= startChr && (int)expression.Entity.ChromosomeId <= endChr)
            .ToArrayAsync();
    }

    private bool HasSsms(int specimenId)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return dbContext.Set<SSM.VariantEntry>()
            .AsNoTracking()
            .Any(entry => entry.AnalysedSample.TargetSampleId == specimenId);
    }

    private bool HasCnvs(int specimenId)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return dbContext.Set<CNV.VariantEntry>()
            .AsNoTracking()
            .Any(entry => entry.AnalysedSample.TargetSampleId == specimenId);
    }

    private bool HasSvs(int specimenId)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return dbContext.Set<SV.VariantEntry>()
            .AsNoTracking()
            .Any(entry => entry.AnalysedSample.TargetSampleId == specimenId);
    }

    private bool HasExpressions(int specimenId)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return dbContext.Set<BulkExpression>()
            .AsNoTracking()
            .Any(expression => expression.AnalysedSample.TargetSampleId == specimenId);
    }
}
