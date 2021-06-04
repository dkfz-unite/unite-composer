using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Criteria.Filters;
using Unite.Composer.Indices.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.OncoGrid;
using Unite.Indices.Entities.Basic.Mutations;
using Unite.Indices.Entities.Donors;

namespace Unite.Composer.Web.Controllers
{
    [Route("api/[controller]")]
    public class OncoGridController : Controller
    {
        private readonly IIndexService<DonorIndex> _donorIndexService;

        public OncoGridController(IIndexService<DonorIndex> donorIndexService)
        {
            _donorIndexService = donorIndexService;
        }

        [HttpPost]
        [CookieAuthorize]
        public OncoGridResource Post([FromBody] SearchCriteria criteria = null)
        {
            var donorSearchResults = _donorIndexService.FindAll(criteria);

            var mostAffectedDonorCount = criteria?.OncoGridFilters.MostAffectedDonorCount ?? 200;
            var mostAffectedGeneCount = criteria?.OncoGridFilters.MostAffectedGeneCount ?? 50;

            var mostAffectedDonors = GetMostAffectedDonors(donorSearchResults, mostAffectedDonorCount);
            var oncoGridDonorResources = mostAffectedDonors.Select(index => new OncoGridDonorResource(index));
            var mostAffectedGeneResources = CreateGenes(mostAffectedDonors, mostAffectedGeneCount);
            var distinctEnsembleIds = mostAffectedGeneResources.Select(res => res.Id);
            var observationResources = CreateObservations(mostAffectedDonors, distinctEnsembleIds);

            var oncoGridResource = new OncoGridResource();
            oncoGridResource.Donors.AddRange(oncoGridDonorResources);
            oncoGridResource.Genes.AddRange(mostAffectedGeneResources);
            oncoGridResource.Observations.AddRange(observationResources);
            return oncoGridResource;
        }

        /// <summary>
        /// Selects the top <see cref="OncoGridFilters.MostAffectedDonorCount"/> Donors ordered by <see cref="DonorIndex.NumberOfMutations"/>
        /// </summary>
        /// <param name="donorSearchResults">Donor searchresult which is limited</param>
        /// <param name="mostAffectedDonorCount">the actual limit of the donors</param>
        /// <returns>A list of unique Donors for the OncoGrid</returns>
        private static List<DonorIndex> GetMostAffectedDonors(SearchResult<DonorIndex> donorSearchResults,
            int mostAffectedDonorCount)
        {
            return donorSearchResults.Rows
                //TODO check whether NumberOfGenes would be better
                .OrderBy(resource => resource.NumberOfMutations)
                .Take(mostAffectedDonorCount)
                .ToList();
        }

        /// <summary>
        /// Selects the top Genes ordered by the occurrence.
        /// 
        /// Based on Donors the actual Mutations.AffectedTranscripts are flattened and grouped by the ensembleID of the transcript-gene.
        /// This genes are then ordered by the count, which represents the actual occurence within mutations.
        /// </summary>
        /// <param name="mostAffectedDonorResources">top X donors of which the affected genes are queried from</param>
        /// <param name="mostAffectedGenes">limit of the resulting unique genes</param>
        /// <returns>A list of unique Genes for the oncogrid</returns>
        private List<OncoGridGeneResource> CreateGenes(IEnumerable<DonorIndex> mostAffectedDonorResources,
            int mostAffectedGenes)
        {
            return mostAffectedDonorResources
                .SelectMany(donorIndex => donorIndex.Mutations
                    .SelectMany(mutation => mutation.AffectedTranscripts?
                        .GroupBy(transcript => new {transcript.Gene.EnsemblId, transcript.Gene.Symbol})
                        .Select(geneGroup => new
                        {
                            GeneResource = new OncoGridGeneResource
                            {
                                Id = geneGroup.Key.EnsemblId,
                                Symbol = geneGroup.Key.Symbol
                            },
                            Count = geneGroup.Count()
                        })
                    ))
                .OrderByDescending(group => group.Count)
                .Select(group => group.GeneResource)
                // TODO: Distinct shouldnt be required because the list is grouped... remove the comparer
                // .Distinct(OncoGridGeneResource.EnsemblIdComparer)
                .Take(mostAffectedGenes)
                .ToList();
            ;
        }

        /// <summary>
        /// Creates a list of <see cref="ObservationResource"/> which represents an oncogrid column entry.
        ///
        /// Based on Donors the actual Mutations.AffectedTranscripts are flattened and filtered by the ensembleid within mostAffectedGenes.
        /// </summary>
        /// <param name="mostAffectedDonorResources">top X donors of which the mutations are prepared for the oncogrid</param>
        /// <param name="mostAffectedGenes">A list of <see cref="GeneIndex.EnsemblId"/> which is used to filter the mutation-transcripts</param>
        /// <returns>A list of column entries for the oncogrid</returns>
        private IEnumerable<ObservationResource> CreateObservations(IEnumerable<DonorIndex> mostAffectedDonorResources,
            IEnumerable<string> mostAffectedGenes)
        {
            return mostAffectedDonorResources
                .SelectMany(donorIndex => donorIndex.Mutations
                    .SelectMany(mutation => mutation.AffectedTranscripts?
                        .Where(transcript => mostAffectedGenes.Contains(transcript.Gene.EnsemblId))
                        .SelectMany(transcript => transcript.Consequences
                            .Select(consequence => new ObservationResource
                            {
                                Type = mutation.Type,
                                DonorId = donorIndex.ReferenceId,
                                GeneId = transcript.Gene.EnsemblId,
                                Consequence = consequence.Type,
                                Id = mutation.Code
                            }))));
        }
    }
}