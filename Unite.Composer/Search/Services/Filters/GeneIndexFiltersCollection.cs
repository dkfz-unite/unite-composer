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
        var donorFilters = new DonorFilters<GeneIndex>(criteria.Donor, gene => gene.Samples.First().Donor);
        var mriImageFilters = new MriImageFilters<GeneIndex>(criteria.Mri, gene => gene.Samples.First().Images.First());
        var tissueFilters = new TissueFilters<GeneIndex>(criteria.Tissue, gene => gene.Samples.First().Specimen);
        var cellLineFilters = new CellLineFilters<GeneIndex>(criteria.Cell, gene => gene.Samples.First().Specimen);
        var organoidFilters = new OrganoidFilters<GeneIndex>(criteria.Organoid, gene => gene.Samples.First().Specimen);
        var xenograftFilters = new XenograftFilters<GeneIndex>(criteria.Xenograft, gene => gene.Samples.First().Specimen);
        var geneFilters = new GeneFilters<GeneIndex>(criteria.Gene, gene => gene);
        var mutationFilters = new MutationFilters<GeneIndex>(criteria.Ssm, gene => gene.Samples.First().Variants.First());
        var copyNumberVariantFilters = new CopyNumberVariantFilters<GeneIndex>(criteria.Cnv, gene => gene.Samples.First().Variants.First());
        var structuralVariantFilters = new StructuralVariantFilters<GeneIndex>(criteria.Sv, gene => gene.Samples.First().Variants.First());

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

        if (criteria.Image != null)
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
              ImageFilterNames.Id,
              gene => gene.Samples.First().Images.First().Id,
              criteria.Image.Id
              ));
        }

        if (criteria.Specimen != null)
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
                SpecimenFilterNames.Id,
                gene => gene.Samples.First().Specimen.Id,
                criteria.Specimen.Id
            ));
        }

        if (criteria.Sample != null)
        {
            _filters.Add(new EqualityFilter<GeneIndex, int>(
                SampleFilterNames.Id,
                gene => gene.Samples.First().Id,
                criteria.Sample.Id
            ));
        }

        if (criteria.Gene != null)
        {
            if (criteria.Gene.HasVariants == true)
            {
                _filters.Add(new GreaterThanFilter<GeneIndex, double?>(
                    GeneFilterNames.HasVariants, 1,
                    gene => gene.NumberOfSsms,
                    gene => gene.NumberOfCnvs,
                    gene => gene.NumberOfSvs
                ));
            }

            if (criteria.Gene.HasExpressions == true)
            {
                _filters.Add(new NotNullFilter<GeneIndex, object>(
                    GeneFilterNames.HasExpressions,
                    gene => gene.Samples.First().Expression
                ));
            }
        }
    }
}
