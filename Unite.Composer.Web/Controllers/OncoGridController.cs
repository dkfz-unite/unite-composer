using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.OncoGrid;
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

        [HttpGet]
        [CookieAuthorize]
        public OncoGridResource Get()
        {
            var donorSearchResults = _donorIndexService.FindAll();
            return CreateData(donorSearchResults);
        }

        [HttpPost]
        [CookieAuthorize]
        public OncoGridResource Post([FromBody] SearchCriteria criteria = null)
        {
            var donorSearchResults = _donorIndexService.FindAll(criteria);
            return CreateData(donorSearchResults);
        }

        private OncoGridResource CreateData(SearchResult<DonorIndex> donorSearchResults)
        {
            var oncoGridResource = new OncoGridResource();
            //TODO Only Top X Donors -> Should be provided as POST-Parameter
            oncoGridResource.Donors.AddRange(donorSearchResults.Rows.Select(index => new OncoGridDonorResource(index)));
            oncoGridResource.Genes.AddRange(CreateGenes(donorSearchResults));
            oncoGridResource.Observations.AddRange(CreateObservations(donorSearchResults));
            return oncoGridResource;
        }

        private IEnumerable<OncoGridGeneResource> CreateGenes(SearchResult<DonorIndex> donorSearchResults)
        {
            //TODO Only Top Y Genes of Top X Donors -> Should be provided as POST-Parameter
            return donorSearchResults.Rows
                .SelectMany(donorIndex => donorIndex.Mutations
                    .SelectMany(mutation => mutation.AffectedTranscripts?
                        .Select(transcript => new OncoGridGeneResource(transcript.Gene))))
                .Distinct(OncoGridGeneResource.EnsemblIdComparer);
            //TODO Should be sorted after a criteria... for example most associated donors?
        }

        private IEnumerable<ObservationResource> CreateObservations(SearchResult<DonorIndex> donorSearchResults)
        {
            //TODO: Top X Donors and Y Genes -> Should be provided as POST-Parameter
            return donorSearchResults.Rows
                .SelectMany(donorIndex => donorIndex.Mutations
                    .SelectMany(mutation => mutation.AffectedTranscripts?
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