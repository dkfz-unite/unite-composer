using Unite.Composer.Search.Engine.Filters;
using Unite.Composer.Search.Services.Criteria;
using Unite.Composer.Search.Services.Filters.Base;
using Unite.Composer.Search.Services.Filters.Constants;
using Unite.Indices.Entities.Genes;

namespace Unite.Composer.Search.Services.Filters;

public class GeneIndexFiltersCollection : FiltersCollection<GeneIndex>
{
    public GeneIndexFiltersCollection(SearchCriteria criteria) : base()
    {
        var donorFilters = new DonorFilters<GeneIndex>(criteria.DonorFilters, gene => gene.Samples.First().Donor);
        var mriImageFilters = new MriImageFilters<GeneIndex>(criteria.MriImageFilters, gene => gene.Samples.First().Images.First());
        var tissueFilters = new TissueFilters<GeneIndex>(criteria.TissueFilters, gene => gene.Samples.First().Specimen);
        var cellLineFilters = new CellLineFilters<GeneIndex>(criteria.CellLineFilters, gene => gene.Samples.First().Specimen);
        var organoidFilters = new OrganoidFilters<GeneIndex>(criteria.OrganoidFilters, gene => gene.Samples.First().Specimen);
        var xenograftFilters = new XenograftFilters<GeneIndex>(criteria.XenograftFilters, gene => gene.Samples.First().Specimen);
        var geneFilters = new GeneFilters<GeneIndex>(criteria.GeneFilters, gene => gene);
        var mutationFilters = new MutationFilters<GeneIndex>(criteria.MutationFilters, gene => gene.Samples.First().Variants.First());
        var copyNumberVariantFilters = new CopyNumberVariantFilters<GeneIndex>(criteria.CopyNumberVariantFilters, gene => gene.Samples.First().Variants.First());
        var structuralVariantFilters = new StructuralVariantFilters<GeneIndex>(criteria.StructuralVariantFilters, gene => gene.Samples.First().Variants.First());

        _filters.AddRange(donorFilters.All());
        _filters.AddRange(mriImageFilters.All());
        _filters.AddRange(tissueFilters.All());
        _filters.AddRange(cellLineFilters.All());
        _filters.AddRange(organoidFilters.All());
        _filters.AddRange(xenograftFilters.All());
        _filters.AddRange(geneFilters.All());
        _filters.AddRange(mutationFilters.All());
        _filters.AddRange(copyNumberVariantFilters.All());
        _filters.AddRange(structuralVariantFilters.All());

        if (criteria.ImageFilters != null)
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
              ImageFilterNames.Id,
              gene => gene.Samples.First().Images.First().Id,
              criteria.ImageFilters.Id
              ));
        }

        if (criteria.SpecimenFilters != null)
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
                SpecimenFilterNames.Id,
                gene => gene.Samples.First().Specimen.Id,
                criteria.SpecimenFilters.Id
            ));
        }

        if (criteria.SampleFilters != null)
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
                SampleFilterNames.Id,
                gene => gene.Samples.First().Id,
                criteria.SampleFilters.Id
            ));
        }

        if (criteria.GeneFilters != null)
        {
            if (criteria.GeneFilters.HasVariants == true)
            {
                _filters.Add(new GreaterThanFilter<GeneIndex, double?>(
                    GeneFilterNames.HasVariants, 1,
                    gene => gene.NumberOfMutations,
                    gene => gene.NumberOfCopyNumberVariants,
                    gene => gene.NumberOfStructuralVariants
                ));
            }

            if (criteria.GeneFilters.HasExpressions == true)
            {
                _filters.Add(new NotNullFilter<GeneIndex, object>(
                    GeneFilterNames.HasExpressions,
                    gene => gene.Samples.First().Expression
                ));
            }
        }
    }
}
