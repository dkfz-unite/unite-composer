using System;
using System.Collections.Generic;
using System.Linq;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Search;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Indices.Services.Configuration.Options;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

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
            // create new search criteria with the mutationId
            var criteria = new SearchCriteria {MutationFilters = new MutationCriteria() {Id = new long[1]}};
            criteria.MutationFilters.Id[0] = mutationId;

            // add filter containing the newly created search criteria
            var filters = new MutationCriteriaFiltersCollection(criteria).All();

            // query the mutations
            var mutationQuery = new SearchQuery<MutationIndex>()
                    .AddFilter(new NotNullFilter<MutationIndex, object>("Mutations.AffectedTranscripts",
                        mutation => mutation.AffectedTranscripts != null))
                    .AddFilters(filters)
                    .AddExclusion(mutation => mutation.Donors.First().Specimens)
                    .AddExclusion(mutation => mutation.Donors.First().Studies)
                .AddExclusion(mutation => mutation.Donors.First().Treatments)
                .AddExclusion(mutation => mutation.Donors.First().ClinicalData)
                .AddExclusion(mutation => mutation.Donors.First().WorkPackages);

            var mutationResult = _mutationService.SearchAsync(mutationQuery).Result;
            return From(mutationResult.Rows);
        }

        private LolliplotData From(IEnumerable<MutationIndex> mutations)
        {
            var proteinList = GetLolliplotProteinData(mutations);
            var mutationList = GetLolliplotMutationData(mutations);
            var lolliplotData = new LolliplotData();
            lolliplotData.Proteins.AddRange(proteinList);
            lolliplotData.Mutations.AddRange(mutationList);

            //TODO return the length of the x-Axis => Max Protein End maybe?
            lolliplotData.DomainWidth = 500;
            // lolliplotData.Transcripts = GetUniqueTranscriptsWithAAChange(mutations);
            lolliplotData.Transcripts = new List<string> {"PPP1R12C-001 (782 aa)", "PPP1R12C-002 (707 aa)", "PPP1R12C-006 (737 aa)", "PPP1R12C-201 (719 aa)"};
            return lolliplotData;
        }

/// <summary>
        /// TODO:
        /// Example display string of icgc: PPP1R12C-001 (782 aa)
        /// Example Ensembl-ID: ENST00000263433
        /// </summary>
        /// <param name="mutations"></param>
        /// <returns></returns>
        private List<string> GetUniqueTranscriptsWithAAChange(IEnumerable<MutationIndex> mutations)
        {
            var uniqueTranscripts = new HashSet<string>();
            foreach (var mutation in mutations)
            {
                foreach (var affectedTranscript in mutation.AffectedTranscripts)
                {
                    if (!string.IsNullOrEmpty(affectedTranscript.AminoAcidChange))
                    {
                        uniqueTranscripts.Add(affectedTranscript.Transcript.EnsemblId);
                    }
                }
            }

            return uniqueTranscripts.ToList();
        }

		private IEnumerable<LolliplotMutationData> GetLolliplotMutationData(IEnumerable<MutationIndex> mutations)        {
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
            
            //Todo extract correct lolliplotMutationData

            foreach (var mutation in mutations)
            {
                mutationList.Add(new LolliplotMutationData
                {
                    Id = mutation.Id.ToString(),
                    Y = 10,
                    X = 10,
                    Consequence = mutation.SequenceType,
                    Impact = "HIGH",
                    Donors = mutation.NumberOfDonors
                });
            }

            /*var random = new Random();
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
            }*/

            return mutationList;
        }

        private static List<LolliplotProteinData> GetLolliplotProteinData(IEnumerable<MutationIndex> mutations)
        {
            var proteinList = new List<LolliplotProteinData>();
            //TODO build ProteinData from mutations. Find out what proteinData is built from
            foreach (var mutation in mutations)
            {
                var proteinData = new LolliplotProteinData()
                {
                    Id = mutation.Alt,
                    Start = mutation.Start,
                    End = mutation.End,
                    Description = mutation.Code
                };
                proteinList.Add(proteinData);
            }
            /*proteinList.Add(new LolliplotProteinData
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
            });*/
            return proteinList;
        }
    }
}