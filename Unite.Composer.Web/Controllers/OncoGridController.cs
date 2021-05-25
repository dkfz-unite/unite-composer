using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Indices;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services;
using Unite.Composer.Resources.Mutations;
using Unite.Composer.Web.Configuration.Filters.Attributes;
using Unite.Composer.Web.Resources.Donors;
using Unite.Composer.Web.Resources.OncoGrid;
using Unite.Indices.Entities.Basic.Mutations;
using Unite.Indices.Entities.Donors;
using MutationIndex = Unite.Indices.Entities.Donors.MutationIndex;

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
        
        [HttpPost]
        [CookieAuthorize]
        public OncoGridResource Post([FromBody]List<int> donorIds = null)
        {
            var donorSearchResults = _donorIndexService.FindAll(criteria);
            return CreateData(donorSearchResults);
        }

        private OncoGridResource CreateData(SearchResult<DonorIndex> donorSearchResults)
        {
            var oncoGridResource = new OncoGridResource();
            oncoGridResource.Donors.AddRange(donorSearchResults.Rows.Select(index => new DonorResource(index)));
            oncoGridResource.Genes.AddRange(CreateGenes(donorSearchResults));
            oncoGridResource.Observations.AddRange(CreateObservations(donorSearchResults));
            return oncoGridResource;
        }

        private IEnumerable<GeneResource> CreateGenes(SearchResult<DonorIndex> donorSearchResults)
        {
            return donorSearchResults.Rows
                .SelectMany(donorIndex => donorIndex.Mutations.SelectMany(mutation => mutation.AffectedTranscripts?
                    .Select(affectedTranscript => new GeneResource(affectedTranscript.Gene))));
        }

        private IEnumerable<ObservationResource> CreateObservations(SearchResult<DonorIndex> donorSearchResults)
        {
            return donorSearchResults.Rows
                .SelectMany(donorIndex => donorIndex.Mutations.SelectMany(mutation => mutation.AffectedTranscripts?
                        .Select(affectedTranscript => new ObservationResource
                        {
                            Type = mutation.Type,
                            DonorId = donorIndex.ReferenceId,
                            Geneid = affectedTranscript.Gene.EnsemblId,
                            Consequence = affectedTranscript.Consequences[0].Type,
                            Id = mutation.Code
                        })));
        }
    }
}