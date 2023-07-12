using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Genome.Variants.Enums;

using DonorIndex = Unite.Indices.Entities.Donors.DonorIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using ImageIndex = Unite.Indices.Entities.Images.ImageIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Donors.DataIndex;

namespace Unite.Composer.Search.Services;

public interface IDonorsSearchService : ISearchService<DonorIndex>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null);
    SearchResult<ImageIndex> SearchImages(int donorId, ImageType type, SearchCriteria searchCriteria = null);
    SearchResult<SpecimenIndex> SearchSpecimens(int donorId, SearchCriteria searchCriteria = null);
    SearchResult<GeneIndex> SearchGenes(int sampleId, SearchCriteria searchCriteria = null);
    SearchResult<VariantIndex> SearchVariants(int sampleId, VariantType type, SearchCriteria searchCriteria = null);
}
