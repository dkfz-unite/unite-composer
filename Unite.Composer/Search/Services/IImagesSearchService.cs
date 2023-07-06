using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Images.DataIndex;

namespace Unite.Composer.Search.Services;

public interface IImagesSearchService : ISearchService<ImageIndex, ImageSearchContext>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
    SearchResult<GeneIndex> SearchGenes(int sampleId, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
    SearchResult<VariantIndex> SearchVariants(int sampleId, VariantType type, SearchCriteria searchCriteria = null, ImageSearchContext searchContext = null);
}
