using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;

namespace Unite.Composer.Search.Services
{
    public interface IDonorsSearchService : ISearchService<DonorIndex>
    {
        SearchResult<MutationIndex> SearchMutations(int donorId, SearchCriteria searchCriteria = null);
        SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null);
    }
}
