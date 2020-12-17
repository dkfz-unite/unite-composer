using System.Linq;
using Nest;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services.Extensions;
using Unite.Indices.Entities.Donors;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Indices.Services
{
    public class DonorIndexService : IndexService<DonorIndex>
    {
        private const string _keywordSuffix = "keyword";

        protected override string DefaultIndex => "donors";

        public DonorIndexService(IElasticOptions options) : base(options)
        {
        }

        protected override ISearchRequest<DonorIndex> CreateRequest(SearchCriteria criteria)
        {
            var request = base.CreateRequest(criteria);

            if (criteria.DonorFilters != null)
            {
                request.AddTermsQuery(
                    donor => donor.Id.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Id
                );

                request.AddMatchQuery(
                    donor => donor.Diagnosis.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Diagnosis
                );

                request.AddTermsQuery(
                    donor => donor.ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender
                );

                request.AddRangeQuery(
                    donor => donor.ClinicalData.Age,
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To
                );

                request.AddTermsQuery(
                    donor => donor.ClinicalData.AgeCategory.Suffix(_keywordSuffix),
                    criteria.DonorFilters.AgeCategory
                );

                request.AddTermsQuery(
                    donor => donor.ClinicalData.VitalStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.VitalStatus
                );
            }

            if (criteria.CellLineFilters != null)
            {
                request.AddTermsQuery(
                    donor => donor.CellLines.First().Name.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Name
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().Type.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Type
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.GeneExpressionSubtype
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().Species.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.Species
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().IdhStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhStatus
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().IdhMutation.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.IdhMutation
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().MethylationStatus.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MethylationStatus
                );

                request.AddTermsQuery(
                    donor => donor.CellLines.First().MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.CellLineFilters.MethylationSubtype
                );

                request.AddBoolQuery(
                    donor => donor.CellLines.First().GcimpMethylation,
                    criteria.CellLineFilters.GcimpMethylation
                );
            }

            if (criteria.MutationFilters != null)
            {
                request.AddTermsQuery(
                    donor => donor.Samples.First().Mutations.First().Code.Suffix(_keywordSuffix),
                    donor => donor.CellLines.First().Samples.First().Mutations.First().Code.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Code
                );

                request.AddTermsQuery(
                    donor => donor.Samples.First().Mutations.First().SequenceType.Suffix(_keywordSuffix),
                    donor => donor.CellLines.First().Samples.First().Mutations.First().SequenceType.Suffix(_keywordSuffix),
                    criteria.MutationFilters.SequenceType
                );

                request.AddTermsQuery(
                    donor => donor.Samples.First().Mutations.First().Type.Suffix(_keywordSuffix),
                    donor => donor.CellLines.First().Samples.First().Mutations.First().Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType
                );

                request.AddTermsQuery(
                    donor => donor.Samples.First().Mutations.First().Contig.Suffix(_keywordSuffix),
                    donor => donor.CellLines.First().Samples.First().Mutations.First().Contig.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Contig
                );

                request.AddTermsQuery(
                    donor => donor.Samples.First().Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    donor => donor.CellLines.First().Samples.First().Mutations.First().Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome
                );

                request.AddRangeQuery(
                    donor => donor.Samples.First().Mutations.First().Position,
                    donor => donor.CellLines.First().Samples.First().Mutations.First().Position,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To
                );
            }

            if (criteria.GeneFilters != null)
            {
                request.AddTermsQuery(
                    donor => donor.Samples.First().Mutations.First().Gene.Name.Suffix(_keywordSuffix),
                    donor => donor.CellLines.First().Samples.First().Mutations.First().Gene.Name.Suffix(_keywordSuffix),
                    criteria.GeneFilters.Name
                );
            }

            return request;
        }
    }
}
