using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Identity.Services;
using Unite.Composer.Search.Services;
using Unite.Composer.Search.Services.Context;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.Models.Identity;
using Unite.Composer.Web.Models.Identity.Validators;
using Unite.Composer.Web.Services.Validation;
using Unite.Data.Entities.Identity;
using Unite.Data.Services;
using Unite.Data.Services.Configuration.Options;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddValidation();

            services.AddScoped<UniteDbContext>();

            services.AddTransient<IAccessibilityService, AccessibilityService>();
            services.AddTransient<IIdentityService<User>, IdentityService>();
            services.AddTransient<ISessionService<User, UserSession>, SessionService>();

            services.AddTransient<ISearchService<Indices.Entities.Donors.DonorIndex>, DonorsSearchService>();
            services.AddTransient<ISearchService<Indices.Entities.Mutations.MutationIndex>, MutationsSearchService>();
            services.AddTransient<ISearchService<Indices.Entities.Specimens.SpecimenIndex, SpecimenSearchContext>, SpecimensSearchService>();

            services.AddTransient<OncoGridDataService>();
        }

        private static void AddOptions(this IServiceCollection services)
        {
            services.AddTransient<IElasticOptions, ElasticOptions>();
            services.AddTransient<ISqlOptions, SqlOptions>();
        }

        private static void AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<SignUpModel>, SignUpModelValidator>();
            services.AddTransient<IValidator<SignInModel>, SignInModelValidator>();
            services.AddTransient<IValidator<PasswordChangeModel>, PasswordChangeModelValidator>();

            services.AddTransient<IValidationService, ValidationService>();
        }
    }
}
