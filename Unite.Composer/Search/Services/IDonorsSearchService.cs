using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context.Enums;
using Unite.Composer.Search.Services.Criteria;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

namespace Unite.Composer.Search.Services;

public interface IDonorsSearchService : ISearchService<DonorIndex>
{
    SearchResult<ImageIndex> SearchImages(int donorId, ImageType type, SearchCriteria searchCriteria = null);
    SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null);
    SearchResult<GeneIndex> SearchGenes(int sampleId, SearchCriteria searchCriteria = null);
    SearchResult<VariantIndex> SearchVariants(int sampleId, VariantType type, SearchCriteria searchCriteria = null);
}
