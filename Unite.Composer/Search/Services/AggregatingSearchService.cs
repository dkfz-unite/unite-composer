namespace Unite.Composer.Search.Services;

using System.Linq.Expressions;
using Unite.Composer.Search.Engine;
using Unite.Composer.Search.Engine.Queries;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters;

using GeneIndex = Unite.Indices.Entities.Genes.GeneIndex;
using VariantIndex = Unite.Indices.Entities.Variants.VariantIndex;

public abstract class AggregatingSearchService
{
    public abstract IIndexService<GeneIndex> GenesIndexService { get; }
    public abstract IIndexService<VariantIndex> VariantsIndexService { get; }


    public int[] AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, SearchCriteria criteria)
    {
        IEnumerable<int> terms = null;

        if (criteria.GeneFilters?.IsNotEmpty() == true)
        {
            var filters = new GeneIndexFiltersCollection(criteria);

            var termsFromGenes =  AggregateFromGenes(property, criteria.Term, filters);

            terms = terms == null ? termsFromGenes : terms.Intersect(termsFromGenes);
        }

        return terms?.ToArray();
    }

    public int[] AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, SearchCriteria criteria)
    {
        IEnumerable<int> terms = null;

        if (criteria.MutationFilters?.IsNotEmpty() == true)
        {
            var filters = new MutationIndexFiltersCollection(criteria with { CopyNumberVariantFilters = null, StructuralVariantFilters = null });

            var termsFromVariants = AggregateFromVariants(property, criteria.Term, filters);

            terms = terms == null ? termsFromVariants : terms.Intersect(termsFromVariants);
        }

        if (criteria.CopyNumberVariantFilters?.IsNotEmpty() == true)
        {
            var filters = new CopyNumberVariantFiltersCollection(criteria with { MutationFilters = null, StructuralVariantFilters = null });

            var termsFromVariants = AggregateFromVariants(property, criteria.Term, filters);

            terms = terms == null ? termsFromVariants : terms.Intersect(termsFromVariants);
        }

        if (criteria.StructuralVariantFilters?.IsNotEmpty() == true)
        {
            var filters = new StructuralVariantFiltersCollection(criteria with { MutationFilters = null, CopyNumberVariantFilters = null });
            
            var termsFromVariants = AggregateFromVariants(property, criteria.Term, filters);

            terms = terms == null ? termsFromVariants : terms.Intersect(termsFromVariants);
        }

        return terms?.ToArray();
    }


    private int[] AggregateFromGenes<TProp>(Expression<Func<GeneIndex, TProp>> property, string term, GeneIndexFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<GeneIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens);

        var result = GenesIndexService.SearchAsync(query).Result;

        var terms = result.Aggregations[aggregationName].Keys
            .Select(key => int.Parse(key))
            .ToArray();

        return terms;
    }

    private int[] AggregateFromVariants<TProp>(Expression<Func<VariantIndex, TProp>> property, string term, VariantFiltersCollection filters)
    {
        var aggregationName = Guid.NewGuid().ToString();

        var query = new SearchQuery<VariantIndex>()
            .AddPagination(0, 0)
            .AddFullTextSearch(term)
            .AddFilters(filters.All())
            .AddAggregation(aggregationName, property)
            .AddExclusion(index => index.Specimens);

        var result = VariantsIndexService.SearchAsync(query).Result;

        var terms = result.Aggregations[aggregationName].Keys
            .Select(key => int.Parse(key))
            .ToArray();

        return terms;
    }
}