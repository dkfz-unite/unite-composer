using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services
{
    public interface IMutationsSearchService : ISearchService<MutationIndex>
    {
        SearchResult<DonorIndex> SearchDonors(long mutationId, SearchCriteria searchCriteria = null);
    }
}
