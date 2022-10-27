using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public interface IGenesSearchService : ISearchService<GeneIndex>
{
    SearchResult<DonorIndex> SearchDonors(int geneId, SearchCriteria searchCriteria = null);
    SearchResult<VariantIndex> SearchVariants(int geneId, VariantType type, SearchCriteria searchCriteria = null);
}
