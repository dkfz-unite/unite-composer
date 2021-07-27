using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Engine.Queries;
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

        public LolliplotData GetData(long mutationId, long selectedTranscriptId)
        {
            var originalMutation = GetMutationById(mutationId);
            var allMutationsWithSelectedTranscript = GetAllMutationsByTranscript(selectedTranscriptId);

            return From(allMutationsWithSelectedTranscript, originalMutation, selectedTranscriptId);
        }

        private LolliplotData From(IEnumerable<MutationIndex> mutations, MutationIndex originalMutation,
            long selectedTranscriptId)
        {
            var lolliplotData = new LolliplotData
            {
                Transcripts = GetUniqueTranscriptsWithAAChange(originalMutation)
            };

            var mutationIndices = mutations.ToList();
            var proteinList = GetLolliplotProteinData(mutationIndices);
            lolliplotData.Proteins.AddRange(proteinList);

            var mutationList = GetLolliplotMutationData(mutationIndices, selectedTranscriptId).ToList();
            if (mutationList.Count == 0)
            {
                return lolliplotData;
            }
            lolliplotData.Mutations.AddRange(mutationList);
            lolliplotData.DomainWidth = mutationList.Max(mutation => mutation.X) + 20;
            return lolliplotData;
        }

        /// <summary>
        /// Gets an id and returns the matching MutationIndex.
        /// </summary>
        /// <param name="mutationId"></param>
        /// <returns cref="MutationIndex">Matching MutationIndex.</returns>
        private MutationIndex GetMutationById(long mutationId)
        {
            var originalMutationQuery = new GetQuery<MutationIndex>(mutationId.ToString());
            return _mutationService.GetAsync(originalMutationQuery).Result;
        }

        /// <summary>
        /// Example display string of icgc: PPP1R12C-001 (782 aa)
        /// Example Ensembl-ID: ENST00000263433
        /// </summary>
        /// <param name="originalMutation"></param>
        /// <returns>All transcripts with amino acid change of the mutation. Contains all transcripts in the top left drawer of the lolliplot.</returns>
        private List<TranscriptData> GetUniqueTranscriptsWithAAChange(MutationIndex originalMutation)
        {
            // sort by display value
            var uniqueTranscripts = new SortedSet<TranscriptData>(TranscriptData.LabelComparer);
            foreach (var affectedTranscript in originalMutation.AffectedTranscripts)
            {
                if (!string.IsNullOrEmpty(affectedTranscript.AminoAcidChange))
                {
                    uniqueTranscripts.Add(new TranscriptData
                    {
                        Label = affectedTranscript.Transcript.EnsemblId,
                        Value = affectedTranscript.Transcript.Id
                    });
                }
            }

            return uniqueTranscripts.ToList();
        }

        /// <summary>
        /// Lolliplot needs to display all mutations that have the same gene in their Transcripts
        /// as the original mutations currently selected AffectedTranscript.
        /// </summary>
        /// <returns>All mutations that have the same transcripts as the original mutations AffectedTranscript including the original mutation.</returns>
        private IEnumerable<MutationIndex> GetAllMutationsByTranscript(long selectedTranscriptId)
        {
            var searchQuery = new SearchQuery<MutationIndex>();
            var transcriptFilter = new EqualityFilter<MutationIndex, long>("Transcript.Id",
                index => index.AffectedTranscripts.First().Transcript.Id, selectedTranscriptId);
            searchQuery.AddFilter(transcriptFilter);
            searchQuery.AddExclusion(x => x.Donors);
            searchQuery.AddPagination(0, 10000);

            var searchResult = _mutationService.SearchAsync(searchQuery).Result;
            return searchResult.Rows;
        }


        /// <summary>
        /// Fills the lolliplot mutation data.
        /// </summary>
        /// <param name="mutations">All mutations relevant for the lolliplot.</param>
        /// <param name="selectedTranscriptId">The currently selected AffectedTranscript id of the original mutation.</param>
        /// <returns cref="LolliplotMutationData">Data for the "lollis" of the lolliplot.</returns>
        private IEnumerable<LolliplotMutationData> GetLolliplotMutationData(IEnumerable<MutationIndex> mutations,
            long selectedTranscriptId)
        {
            var mutationList = new List<LolliplotMutationData>();

            foreach (var mutation in mutations)
            {
                var affectedTranscriptIndex =
                    mutation.AffectedTranscripts.First(t => t.Transcript.Id == selectedTranscriptId);
                
                //TODO QuickFix: affectedTranscriptIndex.AminoAcidChange can be null and leads to an exception.
                if (affectedTranscriptIndex.AminoAcidChange == null)
                {
                    continue;
                }

                var resultString = Regex.Match(affectedTranscriptIndex.AminoAcidChange, @"\d+").Value;
                var xPosition = int.Parse(resultString);
                var consequence = affectedTranscriptIndex.Consequences
                    .OrderBy(consequenceIndex => consequenceIndex.Severity).First();
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

            //TODO get the Protein data from external API

            // Mock Data
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