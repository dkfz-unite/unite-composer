using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Indices.Services;
using Unite.Data.Services.Configuration.Options;
using Unite.Indices.Services.Configuration.Options;
using Unite.Composer.Web.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddOptions();

            services.AddTransient<IIndexService<Unite.Indices.Entities.Donors.DonorIndex>, DonorIndexService>();
            services.AddTransient<IIndexService<Unite.Indices.Entities.Mutations.MutationIndex>, MutationIndexService>();
        }

        private static void AddOptions(this IServiceCollection services)
        {
            services.AddTransient<IElasticOptions, ElasticOptions>();
            services.AddTransient<IMySqlOptions, MySqlOptions>();
        }
    }
}
