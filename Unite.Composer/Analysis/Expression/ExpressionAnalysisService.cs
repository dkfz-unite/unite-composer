using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Analysis.Configuration.Options;
using Unite.Composer.Analysis.Expression.Models;
using Unite.Composer.Analysis.Models;
using Unite.Composer.Analysis.Models.Enums;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Data.Entities.Genome;
using Unite.Data.Entities.Genome.Transcriptomics;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Analysis.Expression;

public class ExpressionAnalysisService : AnalysisService<Models.Analysis, string>
{
    private readonly IAnalysisOptions _options;
    private readonly IDonorsSearchService _donorsSearchService;
    private readonly IImagesSearchService _imagesSearchService;
    private readonly ISpecimensSearchService _specimensSearchService;
    private readonly IDbContextFactory<DomainDbContext> _dbContextFactory;

    private const string _geneIdColumnName = "gene_id";
    private const string _sampleIdColumnName = "sample_id";
    private const string _conditionColumnName = "condition";
    private const string _dataFileNameTemplate = "{0}_data.tsv";
    private const string _metadataFileNameTemplate = "{0}_metadata.tsv";
    private const string _resultsFileNameTemplate = "{0}_results.tsv";
    private const string _resultsFinalFileNameTemplate = "{0}_results_final.tsv";

    public ExpressionAnalysisService(
        IAnalysisOptions options,
        IDonorsSearchService donorsSearchService,
        IImagesSearchService imagesSearchService,
        ISpecimensSearchService specimensSearchService,
        IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _options = options;
        _donorsSearchService = donorsSearchService;
        _imagesSearchService = imagesSearchService;
        _specimensSearchService = specimensSearchService;
        _dbContextFactory = dbContextFactory;
    }


    public override async Task<AnalysisTaskResult> Prepare(Models.Analysis model)
    {
        var stopwatch = new Stopwatch();
        var sampleNamesByCohort = new Dictionary<string, string[]>();
        var sampleExpressionsByGene = new Dictionary<string, Dictionary<string, int>>();

        stopwatch.Restart();

        foreach (var cohort in model.Cohorts.OrderBy(cohort => cohort.Order))
        {
            var cohortSampleExpressionsByGene = await LoadCohortData(cohort);
            
            var cohortSampleNames = cohortSampleExpressionsByGene.Values
                .SelectMany(geneSampleGroups => geneSampleGroups.Keys)
                .Distinct()
                .ToArray();

            sampleNamesByCohort.Add(cohort.Key, cohortSampleNames);

            foreach (var geneGroup in cohortSampleExpressionsByGene)
            {
                if (!sampleExpressionsByGene.ContainsKey(geneGroup.Key))
                    sampleExpressionsByGene.Add(geneGroup.Key, new Dictionary<string, int>());

                foreach (var sampleGroup in geneGroup.Value)
                {
                    if (!sampleExpressionsByGene[geneGroup.Key].ContainsKey(sampleGroup.Key))
                        sampleExpressionsByGene[geneGroup.Key].Add(sampleGroup.Key, sampleGroup.Value);
                }
            }
        }

        await CreateDataFile(sampleNamesByCohort, sampleExpressionsByGene, model.Key);
        await CreateMetadataFile(sampleNamesByCohort, sampleExpressionsByGene, model.Key);

        stopwatch.Stop();

        return AnalysisTaskResult.Success(stopwatch.Elapsed.TotalSeconds);
    }

    public override Task<AnalysisTaskResult> Process(string key)
    {
        var url = $"{_options.DESeq2Url}/api/run?key={key}";

        return ProcessRemotely(url);
    }

    public override async Task<string> LoadResult(string key)
    {
        var fileName = string.Format(_resultsFileNameTemplate, key);
        var filePath = Path.Join(_options.DataPath, fileName);

        var fileNameFinal = string.Format(_resultsFinalFileNameTemplate, key);
        var filePathFinal = Path.Join(_options.DataPath, fileNameFinal);

        if (File.Exists(filePathFinal))
        {
            // Read the final results
            return await File.ReadAllTextAsync(filePathFinal);   
        }
        else if (File.Exists(filePath))
        {
            // Compress the results, save and return them
            using var dbContext = _dbContextFactory.CreateDbContext();

            var genesMap = dbContext.Set<Gene>()
                .ToDictionary(
                    gene => gene.StableId, 
                    gene => (Id: gene.Id, Symbol: gene.Symbol ?? gene.StableId)
                );

            var mapRaw = new ClassMap<AnalysisResults>()
                .Map(x => x.GeneStableId, "ID")
                .Map(x => x.Log2FoldChange, "log2FoldChange")
                .Map(x => x.PValueAdjusted, "padj");

            var mapFinal = new ClassMap<AnalysisResults>()
                .Map(x => x.GeneId, "geneId")
                .Map(x => x.GeneStableId, "geneStableId")
                .Map(x => x.GeneSymbol, "geneSymbol")
                .Map(x => x.Log2FoldChange, "log2FoldChange")
                .Map(x => x.PValueAdjusted, "pValueAdjusted");

            var tsvRaw = await File.ReadAllTextAsync(filePath);

            var dataRaw = TsvReader.Read(tsvRaw, mapRaw);

            var dataFinal = dataRaw.Select(x => new AnalysisResults
            {
                GeneId = genesMap[x.GeneStableId].Id,
                GeneStableId = x.GeneStableId,
                GeneSymbol = genesMap[x.GeneStableId].Symbol,
                Log2FoldChange = x.Log2FoldChange,
                PValueAdjusted = x.PValueAdjusted
            });

            var tsvFinal = TsvWriter.Write(dataFinal, mapFinal);

            await File.WriteAllTextAsync(filePathFinal, tsvFinal);

            return tsvFinal;
        }
        else
        {
            // Return no results
            return null;
        }
    }

    public override Task<string> DownloadResult(string key)
    {
        var fileNameFinal = string.Format(_resultsFinalFileNameTemplate, key);
        var filePathFinal = Path.Join(_options.DataPath, fileNameFinal);
        if (File.Exists(filePathFinal))
            return File.ReadAllTextAsync(filePathFinal);

        return null;        
    }

    public override Task DeleteData(string key)
    {
        var dataFileName = string.Format(_dataFileNameTemplate, key);
        var dataFilePath = Path.Join(_options.DataPath, dataFileName);
        if (File.Exists(dataFilePath))
            File.Delete(dataFilePath);

        var metadataFileName = string.Format(_metadataFileNameTemplate, key);
        var metadataFilePath = Path.Join(_options.DataPath, metadataFileName);
        if (File.Exists(metadataFilePath))
            File.Delete(metadataFilePath);

        var resultsFileName = string.Format(_resultsFileNameTemplate, key);
        var resultsFilePath = Path.Join(_options.DataPath, resultsFileName);
        if (File.Exists(resultsFilePath))
            File.Delete(resultsFilePath);

        var resultsFinalFileName = string.Format(_resultsFinalFileNameTemplate, key);
        var resultsFinalFilePath = Path.Join(_options.DataPath, resultsFinalFileName);
        if (File.Exists(resultsFinalFilePath))
            File.Delete(resultsFinalFilePath);

        return Task.CompletedTask;
    }


    private Task CreateDataFile(Dictionary<string, string[]> samplesMap, Dictionary<string, Dictionary<string, int>> expressionsMap, string key)
    {
        var samples = samplesMap.Values.SelectMany(values => values).Distinct().ToArray();

        var tsv = new StringBuilder();
        tsv.Append($"{_geneIdColumnName}\t");
        tsv.Append(string.Join('\t', samples));
        tsv.Append(Environment.NewLine);

        foreach (var geneId in expressionsMap.Keys)
        {
            var expressions = samples.Select(sampleId => expressionsMap[geneId].TryGetValue(sampleId, out var value) ? value : (int?)null);

            if (ValidateGeneExpressions(expressions))
            {
                tsv.Append($"{geneId}\t");
                tsv.Append(string.Join('\t', expressions));
                tsv.Append(Environment.NewLine);
            }
        }

        var fileName = string.Format(_dataFileNameTemplate, key);
        var filePath = Path.Join(_options.DataPath, fileName);
        return File.WriteAllTextAsync(filePath, tsv.ToString());
    }

    private Task CreateMetadataFile(Dictionary<string, string[]> samplesMap, Dictionary<string, Dictionary<string, int>> expressionsMap, string key)
    {
        var tsv = new StringBuilder();
        tsv.Append($"{_sampleIdColumnName}\t");
        tsv.Append($"{_conditionColumnName}");
        tsv.Append(Environment.NewLine);

        foreach (var cohort in samplesMap)
        {
            foreach (var sampleId in cohort.Value)
            {
                tsv.AppendLine(string.Join('\t', sampleId, cohort.Key));
            }
        }

        var fileName = string.Format(_metadataFileNameTemplate, key);
        var filePath = Path.Join(_options.DataPath, fileName);
        return File.WriteAllTextAsync(filePath, tsv.ToString());
    }

    private Task<Dictionary<string, Dictionary<string, int>>> LoadCohortData(DatasetCriteria model)
    {
        return model.Domain switch
        {
            DatasetDomain.Donors => LoadDonorCohortData(model),
            DatasetDomain.Mris => LoadImageCohortData(model),
            DatasetDomain.Tissues => LoadSpecimenCohortData(model),
            DatasetDomain.Cells => LoadSpecimenCohortData(model),
            DatasetDomain.Organoids => LoadSpecimenCohortData(model),
            DatasetDomain.Xenografts => LoadSpecimenCohortData(model),
            _ => throw new NotSupportedException()
        };
    }

    private async Task<Dictionary<string, Dictionary<string, int>>> LoadDonorCohortData(DatasetCriteria model)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var donorIds = _donorsSearchService.Stats(model.Criteria).Keys.ToArray();

        var specimens = await dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(expression => expression.AnalysedSample.Sample.Specimen.Tissue)
            .Include(expression => expression.AnalysedSample.Sample.Specimen.CellLine)
            .Include(expression => expression.AnalysedSample.Sample.Specimen.Organoid)
            .Include(expression => expression.AnalysedSample.Sample.Specimen.Xenograft)
            .Where(expression => expression.AnalysedSample.Sample.Specimen.ParentId == null)
            .Where(expression => donorIds.Contains(expression.AnalysedSample.Sample.Specimen.DonorId))
            .Select(expression => expression.AnalysedSample.Sample.Specimen)
            .ToArrayAsync();

        var specimensToDonorsMap = specimens
            .GroupBy(specimen => specimen.DonorId)
            .Select(donorGroup => 
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TumorTypeId == TumorType.Primary) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TumorTypeId == TumorType.Metastasis) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TumorTypeId == TumorType.Recurrent) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TypeId == TissueType.Tumor) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TypeId == TissueType.Control) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue != null) ??
                donorGroup.FirstOrDefault(specimen => specimen.CellLine != null) ??
                donorGroup.FirstOrDefault(specimen => specimen.Organoid != null) ??
                donorGroup.FirstOrDefault(specimen => specimen.Xenograft != null) ??
                donorGroup.FirstOrDefault())
            .ToDictionary(specimen => specimen.Id, specimen => specimen.DonorId);

        var specimenIds = specimensToDonorsMap.Keys.ToArray();

        var expressions = await dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(expression => expression.Gene)
            .Include(expression => expression.AnalysedSample.Sample)
            .OrderBy(expression => expression.Gene.ChromosomeId)
            .ThenBy(expression => expression.Gene.Start)
            .Where(expression => specimenIds.Contains(expression.AnalysedSample.Sample.SpecimenId))
            .ToArrayAsync();

        var getSampleId = (GeneExpression expression) =>
        {
            var donorId = specimensToDonorsMap[expression.AnalysedSample.Sample.SpecimenId];
            var sampleId = $"{model.Domain}_{donorId}";
            return sampleId;
        };

        var expressionsToDonorsMap = expressions
            .GroupBy(expression => expression.Gene.StableId)
            .ToDictionary(
                geneGroup => geneGroup.Key, 
                geneGroup => geneGroup.ToDictionary(
                    expression => getSampleId(expression), 
                    expression => expression.Reads
                )
            );

        return expressionsToDonorsMap;
    }

    private async Task<Dictionary<string, Dictionary<string, int>>> LoadImageCohortData(DatasetCriteria model)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var context = GetImageSearchContext(model.Domain);

        var imageIds = _imagesSearchService.Stats(model.Criteria, context).Keys.ToArray();

        var images = await dbContext.Set<Image>()
            .AsNoTracking()
            .Where(image => imageIds.Contains(image.Id))
            .ToArrayAsync();

        var imagesToDonorsMap = images
            .ToDictionary(image => image.Id, image => image.DonorId);

        var donorIds = imagesToDonorsMap.Values.Distinct().ToArray();

        var specimens = await dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(expression => expression.AnalysedSample.Sample.Specimen.Tissue)
            .Where(expression => expression.AnalysedSample.Sample.Specimen.ParentId == null)
            .Where(expression => expression.AnalysedSample.Sample.Specimen.Tissue != null)
            .Where(expression => donorIds.Contains(expression.AnalysedSample.Sample.Specimen.DonorId))
            .Select(expression => expression.AnalysedSample.Sample.Specimen)
            .ToArrayAsync();

        var specimensToDonorsMap = specimens
            .GroupBy(specimen => specimen.DonorId)
            .Select(donorGroup => 
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TumorTypeId == TumorType.Primary) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TumorTypeId == TumorType.Metastasis) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TumorTypeId == TumorType.Recurrent) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TypeId == TissueType.Tumor) ??
                donorGroup.FirstOrDefault(specimen => specimen.Tissue?.TypeId == TissueType.Control) ??
                donorGroup.FirstOrDefault())
            .ToDictionary(specimen => specimen.Id, specimen => specimen.DonorId);

        var specimenIds = specimensToDonorsMap.Keys.ToArray();

        var expressions = await dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(expression => expression.Gene)
            .Include(expression => expression.AnalysedSample.Sample)
            .OrderBy(expression => expression.Gene.ChromosomeId)
            .ThenBy(expression => expression.Gene.Start)
            .Where(expression => specimenIds.Contains(expression.AnalysedSample.Sample.SpecimenId))
            .ToArrayAsync();

        var getSampleId = (GeneExpression expression) =>
        {
            var donorId = specimensToDonorsMap[expression.AnalysedSample.Sample.SpecimenId];
            var imageId = imagesToDonorsMap.First(entry => entry.Value == donorId).Key;
            var sampleId = $"{model.Domain}_{imageId}";
            return sampleId;
        };

        var expressionsToImagesMap = expressions
            .GroupBy(expression => expression.Gene.StableId)
            .ToDictionary(
                geneGroup => geneGroup.Key, 
                geneGroup => geneGroup.ToDictionary(
                    expression => getSampleId(expression), 
                    expression => expression.Reads
                )
            );

        return expressionsToImagesMap;
    }

    private async Task<Dictionary<string, Dictionary<string, int>>> LoadSpecimenCohortData(DatasetCriteria model)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var context = GetSpecimenSearchContext(model.Domain);

        var specimenIds = _specimensSearchService.Stats(model.Criteria, context).Keys.ToArray();

        var expressions = await dbContext.Set<GeneExpression>()
            .AsNoTracking()
            .Include(expression => expression.Gene)
            .Include(expression => expression.AnalysedSample.Sample)
            .OrderBy(expression => expression.Gene.ChromosomeId)
            .ThenBy(expression => expression.Gene.Start)
            .Where(expression => specimenIds.Contains(expression.AnalysedSample.Sample.SpecimenId))
            .ToArrayAsync();

        var getSampleId = (GeneExpression expression) =>
        {
            var specimenId = expression.AnalysedSample.Sample.SpecimenId;
            var sampleId = $"{model.Domain}_{specimenId}";
            return sampleId;
        };

        var expressionsToSpecimensMap = expressions
            .GroupBy(expression => expression.Gene.StableId)
            .ToDictionary(
                geneGroup => geneGroup.Key, 
                geneGroup => geneGroup.ToDictionary(
                    expression => getSampleId(expression), 
                    expression => expression.Reads
                )
            );

        return expressionsToSpecimensMap;
    }

    private static bool ValidateGeneExpressions(IEnumerable<int?> expressions)
    {
        var results = new List<bool>();

        // Should have no null values
        results.Add(expressions.All(expression => expression.HasValue));

        // Should have 50% values greater than 10
        results.Add(expressions.Count(expression => expression > 10) > expressions.Count() / 2);

        return results.All(result => result);
    }

    private static ImageSearchContext GetImageSearchContext(DatasetDomain type)
    {
        return type switch
        {
            DatasetDomain.Mris => new ImageSearchContext(ImageType.MRI),
            _ => throw new NotSupportedException()
        };
    }

    private static SpecimenSearchContext GetSpecimenSearchContext(DatasetDomain type)
    {
        return type switch
        {
            DatasetDomain.Tissues => new SpecimenSearchContext(SpecimenType.Tissue),
            DatasetDomain.Cells => new SpecimenSearchContext(SpecimenType.CellLine),
            DatasetDomain.Organoids => new SpecimenSearchContext(SpecimenType.Organoid),
            DatasetDomain.Xenografts => new SpecimenSearchContext(SpecimenType.Xenograft),
            _ => throw new NotSupportedException()
        };
    }
}
