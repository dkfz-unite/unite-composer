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

            request.AddTermsQuery(
                mutation => mutation.SequenceType.Suffix(_keywordSuffix),
                new string[] { "g" }
            );

            if (criteria.MutationFilters != null)
            {
                request.AddMatchQuery(
                    mutation => mutation.Name,
                    criteria.MutationFilters.Name
                );

                request.AddMatchQuery(
                    mutation => mutation.Code,
                    criteria.MutationFilters.Code
                );

                //request.AddTermsQuery(
                //    mutation => mutation.SequenceType.Suffix(_keywordSuffix),
                //    criteria.MutationFilters.SequenceType
                //);

                request.AddTermsQuery(
                    mutation => mutation.Type.Suffix(_keywordSuffix),
                    criteria.MutationFilters.MutationType
                );

                request.AddMatchQuery(
                    mutation => mutation.Contig,
                    criteria.MutationFilters.Contig
                );

                request.AddTermsQuery(
                    mutation => mutation.Chromosome.Suffix(_keywordSuffix),
                    criteria.MutationFilters.Chromosome
                );

                request.AddRangeQuery(
                    mutation => mutation.Position,
                    criteria.MutationFilters.Position?.From,
                    criteria.MutationFilters.Position?.To
                );
            }

            if (criteria.GeneFilters != null)
            {
                request.AddMatchQuery(
                    mutation => mutation.Gene.Name,
                    criteria.GeneFilters.Name
                );
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

            //if (criteria.CellLineFilters != null)
            //{
            //    request.AddMatchQuery(
            //        mutation => mutation.Samples.First().CellLine.Name,
            //        criteria.CellLineFilters.Name
            //    );

            //    request.AddTermsQuery(
            //        mutation => mutation.Samples.First().CellLine.Type.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.Type
            //    );

            //    request.AddTermsQuery(
            //        mutation => mutation.Samples.First().CellLine.Species.Suffix(_keywordSuffix),
            //        criteria.CellLineFilters.Species
            //    );
            //}

            request.OrderBy(
                mutation => mutation.Id,
                SortOrder.Ascending
            );

            return request;
        }
    }
}
