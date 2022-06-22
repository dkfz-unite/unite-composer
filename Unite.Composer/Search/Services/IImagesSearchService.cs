using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services;

public interface IImagesSearchService : ISearchService<ImageIndex, ImageSearchContext>
{
    SearchResult<GeneIndex> SearchGenes(int imageId, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
    SearchResult<MutationIndex> SearchMutations(int imageId, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
}
