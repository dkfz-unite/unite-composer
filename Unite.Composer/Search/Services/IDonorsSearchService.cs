using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;

using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services
{
    public interface IDonorsSearchService : ISearchService<DonorIndex>
    {
        SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null);
        SearchResult<GeneIndex> SearchGenes(int donorId, SearchCriteria searchCriteria = null);
        SearchResult<MutationIndex> SearchMutations(int donorId, SearchCriteria searchCriteria = null);
        SearchResult<ImageIndex> SearchImages(int donorId, SearchCriteria searchCriteria = null);
    }
}
