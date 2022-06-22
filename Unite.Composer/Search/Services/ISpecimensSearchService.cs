using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;

using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using MutationIndex = Unite.Indices.Entities.Mutations.MutationIndex;

namespace Unite.Composer.Search.Services;

public interface ISpecimensSearchService : ISearchService<SpecimenIndex, SpecimenSearchContext>
{
    SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
    SearchResult<MutationIndex> SearchMutations(int specimenId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
}
