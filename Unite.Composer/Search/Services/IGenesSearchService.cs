using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services;

public interface IGenesSearchService : ISearchService<GeneIndex>
{
    SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null);
    SearchResult<MutationIndex> SearchMutations(int geneId, SearchCriteria searchCriteria = null);
}
