﻿using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Search.Services.Criteria;
using Unite.Data.Entities.Genome.Variants.Enums;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using SpecimenIndex = Unite.Indices.Entities.Specimens.SpecimenIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;
using DataIndex = Unite.Indices.Entities.Specimens.DataIndex;

namespace Unite.Composer.Search.Services;

public interface ISpecimensSearchService : ISearchService<SpecimenIndex, SpecimenSearchContext>
{
    IDictionary<int, DataIndex> Stats(SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
    SearchResult<GeneIndex> SearchGenes(int specimenId, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
    SearchResult<VariantIndex> SearchVariants(int specimenId, VariantType type, SearchCriteria searchCriteria = null, SpecimenSearchContext searchContext = null);
}
