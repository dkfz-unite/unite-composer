using System;
using System.Collections.Generic;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Indices.Entities.Mutations;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Visualization.Lolliplot
{
    public class LolliplotDataService
    {
        private readonly IIndexService<MutationIndex> _mutationService;

        public LolliplotDataService(IElasticOptions options)
        {
            _mutationService = new MutationsIndexService(options);
        }

        public LolliplotData GetData(SearchCriteria searchCriteria = null)
        {
            // var criteria = searchCriteria ?? new SearchCriteria();
            //
            // var filters = new MutationCriteriaFiltersCollection(criteria).All();
            //
            // var mutationQuery = new SearchQuery<MutationIndex>()
            //     .AddFilter(new NotNullFilter<MutationIndex, object>("Mutations.AffectedTranscripts",
            //         mutation => mutation.AffectedTranscripts != null))
            //     .AddFilters(filters)
            //     .AddExclusion(mutation => mutation.Donors.First().Specimens);
            // //TODO: exclude all unnecessary information as soon as multiple exclusions work.
            // // .AddExclusion(mutation => mutation.Donors.First().Studies)
            // // .AddExclusion(mutation => mutation.Donors.First().Treatments)
            // // .AddExclusion(mutation => mutation.Donors.First().ClinicalData)
            // // .AddExclusion(mutation => mutation.Donors.First().WorkPackages);
            //
            // var mutationResult = _mutationService.SearchAsync(mutationQuery).Result;

            return From(null);
        }

        private LolliplotData From(IEnumerable<MutationIndex> mutations)
        {
            var proteinList = GetLolliplotProteinData();
            var mutationList = GetLolliplotMutationData();
            var lolliplotData = new LolliplotData();
            lolliplotData.Proteins.AddRange(proteinList);
            lolliplotData.Mutations.AddRange(mutationList);
            return lolliplotData;
        }

        private IEnumerable<LolliplotMutationData> GetLolliplotMutationData()
        {
            var mutationList = new List<LolliplotMutationData>();

            var consequenceConst = new List<string>(new[]
            {
                "Transcript ablation",
                "Splice acceptor",
                "Splice donor",
                "Stop gained",
                "Frameshift",
                "Stop lost",
                "Start lost",
                "Transcript amplification",
                "Inframe insertion",
                "Inframe deletion",
                "Missense",
                "Protein altering",
                "Splice region"
            });

            var impactConst = new List<string>(new[]
            {
                "HIGH",
                "MODERATE",
                "LOW"
            });


            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                mutationList.Add(new LolliplotMutationData
                {
                    Id = "" + i,
                    Y = "" + random.Next(0, 100),
                    X = "" + random.Next(0, 100),
                    Consequence = consequenceConst[random.Next(0, consequenceConst.Count - 1)],
                    Impact = impactConst[random.Next(0, impactConst.Count - 1)],
                    Donors = "" + random.Next(0, 100)
                });
            }

            return mutationList;
        }

        private static List<LolliplotProteinData> GetLolliplotProteinData()
        {
            var proteinList = new List<LolliplotProteinData>();
            proteinList.Add(new LolliplotProteinData
            {
                Id = "TAD",
                Start = "0",
                End = "75",
                Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
                GetProteinColor = "() => 'blue'"
            });
            proteinList.Add(new LolliplotProteinData
            {
                Id = "DNA-binding",
                Start = "125",
                End = "350",
                Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
            });
            proteinList.Add(
                new LolliplotProteinData
                {
                    Id = "Oligomerization",
                    Start = "325",
                    End = "375",
                    Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
                });
            proteinList.Add(new LolliplotProteinData
            {
                Id = "NLS",
                Start = "450",
                End = "500",
                Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
            });
            return proteinList;
        }
    }
}