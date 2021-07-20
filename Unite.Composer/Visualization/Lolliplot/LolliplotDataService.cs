using System;
using System.Collections.Generic;
using System.Linq;
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

        public LolliplotData GetData(long mutationId)
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
            //TODO return the length of the x-Axis => Max Protein End maybe?
            lolliplotData.DomainWidth = 500;
            return lolliplotData;
        }

        private IEnumerable<LolliplotMutationData> GetLolliplotMutationData()
        {
            var mutationList = new List<LolliplotMutationData>();

            var consequenceConst = new List<string>(new[]
            {
                "transcript_ablation",
                "splice_acceptor_variant",
                "splice_donor_variant",
                "stop_gained",
                "frameshift_variant",
                "stop_lost",
                "start_lost",
                "transcript_amplification",
                "inframe_insertion",
                "inframe_deletion",
                "missense_variant",
                "protein_altering_variant",
                "splice_region_variant",
                "incomplete_terminal_codon_variant",
                "start_retained_variant",
                "stop_retained_variant",
                "synonymous_variant",
                "coding_sequence_variant",
                "mature_miRNA_variant",
                "5_prime_UTR_variant",
                "3_prime_UTR_variant",
                "non_coding_transcript_exon_variant",
                "intron_variant",
                "NMD_transcript_variant",
                "non_coding_transcript_variant",
                "upstream_gene_variant",
                "downstream_gene_variant",
                "TFBS_ablation",
                "TFBS_amplification",
                "TF_binding_site_variant",
                "regulatory_region_ablation",
                "regulatory_region_amplification",
                "feature_elongation",
                "regulatory_region_variant",
                "feature_truncation",
                "intergenic_variant"
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
                    Y = random.Next(0, 100),
                    X = random.Next(0, 100),
                    Consequence = consequenceConst[random.Next(0, consequenceConst.Count - 1)],
                    Impact = impactConst[random.Next(0, impactConst.Count - 1)],
                    Donors = random.Next(0, 100)
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
                Start = 0,
                End = 75,
                Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
                GetProteinColor = "() => 'blue'"
            });
            proteinList.Add(new LolliplotProteinData
            {
                Id = "DNA-binding",
                Start = 125,
                End = 350,
                Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
            });
            proteinList.Add(
                new LolliplotProteinData
                {
                    Id = "Oligomerization",
                    Start = 325,
                    End = 375,
                    Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
                });
            proteinList.Add(new LolliplotProteinData
            {
                Id = "NLS",
                Start = 450,
                End = 500,
                Description = "von Hippel-Lindau disease tumour suppressor, beta/alpha domain",
            });
            return proteinList;
        }
    }
}