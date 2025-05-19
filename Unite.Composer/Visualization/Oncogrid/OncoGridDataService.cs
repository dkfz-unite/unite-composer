using Microsoft.EntityFrameworkCore;
using Unite.Composer.Visualization.Oncogrid.Data;
using Unite.Indices.Context.Configuration.Options;
using Unite.Indices.Search.Services;
using Unite.Indices.Search.Services.Filters.Base.Donors.Criteria;
using Unite.Indices.Search.Services.Filters.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Basic.Omics.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.SmIndex;

namespace Unite.Composer.Visualization.Oncogrid;

public class OncoGridDataService
{
    private readonly DonorsSearchService _donorsSearchService;
    private readonly SmsSearchService _smsSearchService;


    public OncoGridDataService(IElasticOptions options)
    {
        _donorsSearchService = new DonorsSearchService(options);
        _smsSearchService = new SmsSearchService(options);
    }


    public OncoGridData LoadData(int numberOfDonors = 30, int numberOfGenes = 50, SearchCriteria searchCriteria = null)
    {
        var criteria = searchCriteria ?? new SearchCriteria();

        var donors = LoadDonors(criteria, numberOfDonors);
        
        var variants = LoadMutations(criteria, donors.Select(donor => donor.Id).ToArray());

        var genes = LoadGenes(variants, numberOfGenes);
            

       return GetOncoGridData(donors, genes, variants);
    }


    private DonorIndex[] LoadDonors(SearchCriteria searchCriteria, int number = 30)
    {
        var criteria = searchCriteria with
        { 
            From = 0, 
            Size = number 
        };

        return _donorsSearchService.Search(criteria).Result.Rows.ToArray();
    }

    private VariantIndex[] LoadMutations(SearchCriteria searchCriteria, int[] donorIds)
    {
        var tasks = new List<Task>();
        // var variants = new List<VariantIndex>();
        var variants = new Dictionary<int, VariantIndex>();

        foreach (var id in donorIds)
        {
            var criteria = searchCriteria with
            {
                From = 0,
                Size = 10000,
                Donor = new DonorCriteria() { Id = [id] }
            };

            var task = _smsSearchService.Search(criteria).ContinueWith(result => 
            {
                var donorVariants = result.Result.Rows;

                foreach (var variant in donorVariants)
                {
                    variant.AffectedFeatures = variant.AffectedFeatures?.Where(affectedFeature => affectedFeature.Gene != null).ToArray();

                    if (variant.AffectedFeatures == null)
                        continue;
                    
                    foreach (var affectedFeature in variant.AffectedFeatures)
                    {
                        affectedFeature.Effects = affectedFeature.Effects
                            .Where(effect => HasMatchingImpact(effect.Impact, searchCriteria.Sm.Impact))
                            .Where(effect => HasMatchingEffects(effect.Type, searchCriteria.Sm.Effect))
                            .OrderBy(effect => effect.Severity)
                            .Take(1)
                            .ToArray();
                    }

                    variant.AffectedFeatures = variant.AffectedFeatures
                        .Where(affectedFeature => affectedFeature.Effects.Any())
                        .OrderBy(affectedFeature => affectedFeature.Effects.First().Severity)
                        .Take(1)
                        .ToArray();

                    if (variant.AffectedFeatures.Any())
                    {
                        lock (variants)
                        {
                            variants.TryAdd(variant.Id, variant);
                        }
                    }
                }

                // lock (variants)
                // {
                //     variants.AddRange(donorVariants.Where(variant => variant.AffectedFeatures.Any()));
                // }
            });
            
            tasks.Add(task);
        }

        Task.WaitAll(tasks.ToArray());

        return variants.Values.ToArray();
    }

    private GeneIndex[] LoadGenes(VariantIndex[] variants, int number = 50)
    {
        return variants
            .GroupBy(variant => variant.AffectedFeatures.First().Gene.Id)
            .Select(group => new { Gene = group.First().AffectedFeatures.First().Gene, Count = group.Count() })
            .OrderByDescending(group => group.Count)
            .Select(group => group.Gene)
            .Take(number)
            .ToArray();
    }


    private static OncoGridData GetOncoGridData(
        IEnumerable<DonorIndex> donors,
        IEnumerable<GeneIndex> genes,
        IEnumerable<VariantIndex> variants)
    {
        var oncoGridData = new OncoGridData();

        // Collections will be enumerated in controller, when building JSON object to return.
        // If immediate enumeration required, call 'ToArray' method for required data set.
        // oncoGridData.Observations = GetObservationsData(oncoGridData.Donors, oncoGridData.Genes, variants, impacts, effects);
        // oncoGridData.Donors = GetDonorsData(donors);
        // oncoGridData.Genes = GetGenesData(genes);

        oncoGridData.Observations = GetObservationsData(donors, genes, variants);
        
        var donorIds = oncoGridData.Observations.Select(observation => int.Parse(observation.DonorId)).Distinct().ToArray();
        oncoGridData.Donors = GetDonorsData(donors, donorIds);

        var geneIds = oncoGridData.Observations.Select(observation => int.Parse(observation.GeneId)).Distinct().ToArray();
        oncoGridData.Genes = GetGenesData(genes, geneIds);

        return oncoGridData;
    }

    private static IEnumerable<OncoGridDonor> GetDonorsData(
        IEnumerable<DonorIndex> donors,
        int[] ids)
    {
        return donors
            .Where(donor => ids.Contains(donor.Id))
            .Select(index => new OncoGridDonor(index));
    }

    private static IEnumerable<OncoGridGene> GetGenesData(
        IEnumerable<GeneIndex> genes,
        int[] ids)
    {
        return genes
            .Where(gene => ids.Contains(gene.Id))
            .Select(gene => new OncoGridGene(gene));
    }

    private static IEnumerable<OncoGridVariant> GetObservationsData(
        IEnumerable<DonorIndex> donors,
        IEnumerable<GeneIndex> genes,
        IEnumerable<VariantIndex> variants)
    {
        foreach (var donor in donors)
        {
            foreach (var gene in genes)
            {
                var observations = variants
                    .Where(variant => variant.Specimens.Select(specimen => specimen.Id).Intersect(donor.Specimens.Select(specimen => specimen.Id)).Any())
                    .Where(variant => variant.AffectedFeatures.First().Gene.Id == gene.Id)
                    .ToArray();

                foreach (var observation in observations)
                {
                    yield return new OncoGridVariant
                    {
                        Id = $"{observation.Id}",
                        Code = GetVariantCode(observation),
                        Effect = observation.AffectedFeatures.First().Effects.First().Type,
                        Impact = observation.AffectedFeatures.First().Effects.First().Impact,
                        DonorId = $"{donor.Id}",
                        GeneId = $"{gene.Id}"
                    };
                }
            }
        }
    }


    private static bool HasMatchingImpact(string impact, IEnumerable<string> impacts)
    {
        return impacts == null || !impacts.Any() || impacts.Contains(impact);
    }

    private static bool HasMatchingEffects(string effect, IEnumerable<string> effects)
    {
        return effects == null || !effects.Any() || effects.Contains(effect);
    }

    private static string GetVariantCode(VariantIndex variant)
    {
        return $"{variant.Chromosome}:g.{variant.Start}{variant.Ref ?? "-"}>{variant.Alt ?? "-"}";
    }
}
