using System.Linq;
using Nest;
using Unite.Composer.Indices.Criteria;
using Unite.Composer.Indices.Services.Extensions;
using Unite.Indices.Entities.Mutations;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Indices.Services
{
    public class MutationIndexService : IndexService<MutationIndex>
    {
        private const string _keywordSuffix = "keyword";

        protected override string DefaultIndex => "mutations";

        public MutationIndexService(IElasticOptions options) : base(options)
        {
        }

        protected override ISearchRequest<MutationIndex> CreateRequest(SearchCriteria criteria)
        {
            var request = base.CreateRequest(criteria);

            if (criteria.MutationFilters != null)
            {
                request.AddMatchQuery(
                    mutation => mutation.Code,
                    criteria.MutationFilters.Code
                );

                request.AddTermsQuery(
                    mutation => mutation.Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType
                );

                request.AddTermsQuery(
                    mutation => mutation.Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome
                );

                request.AddRangeQuery(
                    mutation => mutation.Start,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To
                );

                //request.AddRangeQuery(
                //    mutation => mutation.End,
                //    criteria.MutationFilters.Position?.From,
                //    criteria.MutationFilters.Position?.To
                //);
            }

            if (criteria.GeneFilters != null)
            {
                //request.AddMatchQuery(
                //    mutation => mutation.Gene.Name,
                //    criteria.GeneFilters.Name
                //);
            }

            if (criteria.DonorFilters != null)
            {
                request.AddTermsQuery(
                    mutation => mutation.Donors.First().Id.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Id
                );

                request.AddMatchQuery(
                    mutation => mutation.Donors.First().Diagnosis,
                    criteria.DonorFilters.Diagnosis
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().ClinicalData.Gender.Suffix(_keywordSuffix),
                    criteria.DonorFilters.Gender
                );

                request.AddRangeQuery(
                    mutation => mutation.Donors.First().ClinicalData.Age,
                    criteria.DonorFilters.Age?.From,
                    criteria.DonorFilters.Age?.To
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().ClinicalData.AgeCategory.Suffix(_keywordSuffix),
                    criteria.DonorFilters.AgeCategory
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().ClinicalData.VitalStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.VitalStatus
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().EpigeneticsData.GeneExpressionSubtype.Suffix(_keywordSuffix),
                    criteria.DonorFilters.GeneExpressionSubtype
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().EpigeneticsData.IdhStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.IdhStatus
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().EpigeneticsData.IdhMutation.Suffix(_keywordSuffix),
                    criteria.DonorFilters.IdhMutation
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().EpigeneticsData.MethylationStatus.Suffix(_keywordSuffix),
                    criteria.DonorFilters.MethylationStatus
                );

                request.AddTermsQuery(
                    mutation => mutation.Donors.First().EpigeneticsData.MethylationSubtype.Suffix(_keywordSuffix),
                    criteria.DonorFilters.MethylationSubtype
                );

                request.AddBoolQuery(
                    mutation => mutation.Donors.First().EpigeneticsData.GcimpMethylation,
                    criteria.DonorFilters.GcimpMethylation
                );
            }

            request.OrderBy(
                mutation => mutation.NumberOfDonors,
                SortOrder.Descending
            );

            request.Exclude(
                mutation => mutation.Donors
            );

            return request;
        }
    }
}
