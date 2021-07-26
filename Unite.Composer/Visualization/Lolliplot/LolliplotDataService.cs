using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Composer.Search.Services.Search;
using Unite.Composer.Visualization.Lolliplot.Data;
using Unite.Data.Entities.Mutations;
using Unite.Indices.Entities.Basic.Mutations;
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

        public LolliplotData GetData(long mutationId, long selectedTranscriptId)
        {
            var originalMutationQuery = new GetQuery<MutationIndex>(mutationId.ToString());
            var originalMutation = _mutationService.GetAsync(originalMutationQuery).Result;
            var searchQuery = new SearchQuery<MutationIndex>();
            var transcriptFilter = new EqualityFilter<MutationIndex, long>("Transcript.Id",
                index => index.AffectedTranscripts.First().Transcript.Id, selectedTranscriptId);
            searchQuery.AddFilter(transcriptFilter);
            searchQuery.AddExclusion(x => x.Donors);
            searchQuery.AddPagination(0, 10000);

            var searchResult = _mutationService.SearchAsync(searchQuery).Result;

            return From(searchResult.Rows, originalMutation, selectedTranscriptId);
        }

        private LolliplotData From(IEnumerable<MutationIndex> mutations, MutationIndex originalMutation, long selectedTranscriptId)
        {
            var lolliplotData = new LolliplotData
            {
                Transcripts = GetUniqueTranscriptsWithAAChange(originalMutation)
            };


            // TODO get additional data based on the selected transcript 
            var proteinList = GetLolliplotProteinData(mutations);
            lolliplotData.Proteins.AddRange(proteinList);

            // provide the mutation data to the lolliplot
            var mutationList = GetLolliplotMutationData(mutations, selectedTranscriptId);
            lolliplotData.Mutations.AddRange(mutationList);
            lolliplotData.DomainWidth = mutationList.Max(mutation => mutation.X);
            return lolliplotData;
        }

        /// <summary>
        /// TODO:
        /// Example display string of icgc: PPP1R12C-001 (782 aa)
        /// Example Ensembl-ID: ENST00000263433
        /// </summary>
        /// <param name="originalMutation"></param>
        /// <returns></returns>
        private List<string> GetUniqueTranscriptsWithAAChange(MutationIndex originalMutation)
        {
            var uniqueTranscripts = new HashSet<string>();
            foreach (var affectedTranscript in originalMutation.AffectedTranscripts)
            {
                if (!string.IsNullOrEmpty(affectedTranscript.AminoAcidChange))
                {
                    uniqueTranscripts.Add(affectedTranscript.Transcript.EnsemblId);
                }
            }
            return uniqueTranscripts.ToList();
        }

        /// <summary>
        /// Lolliplot needs to display all mutations that have the same gene in their Transcripts
        /// as the original mutations currently selected AffectedTranscript.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<MutationIndex> GetAllLolliplotMutationsBySelectedGene(MutationIndex originalMutation,
            AffectedTranscriptIndex selectedTranscript)
        {
            //TODO get all the relevant mutations
            var mutations = new List<MutationIndex>
            {
                originalMutation
            };
            var gene = selectedTranscript.Gene;
            // "find all other transcripts across all mutations and all donors, which have protein affected."
            return mutations;
        }

        private IEnumerable<LolliplotMutationData> GetLolliplotMutationData(IEnumerable<MutationIndex> mutations,
            long selectedTranscriptId)
        {
            var mutationList = new List<LolliplotMutationData>();

            foreach (var mutation in mutations)
            {
                var af = mutation.AffectedTranscripts.First(t => t.Transcript.Id == selectedTranscriptId);
                var resultString = Regex.Match(af.AminoAcidChange, @"\d+").Value;
                var xPosition = int.Parse(resultString);
                var consequence = af.Consequences.OrderBy(conseq => conseq.Severity).First();
                mutationList.Add(new LolliplotMutationData
                {
                    Id = mutation.Id.ToString(),
                    Y = mutation.NumberOfDonors,
                    X = xPosition,
                    Consequence = consequence.Type,
                    Impact = consequence.Impact,
                    Donors = mutation.NumberOfDonors
                });
            }

            return mutationList;
        }

        private static List<LolliplotProteinData> GetLolliplotProteinData(IEnumerable<MutationIndex> mutations)
        {
            var proteinList = new List<LolliplotProteinData>();

            //TODO build ProteinData from mutations. Find out what proteinData is built from

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