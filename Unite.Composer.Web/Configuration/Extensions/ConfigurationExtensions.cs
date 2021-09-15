using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Identity.Services;
using Unite.Composer.Search.Services;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.Models.Identity;
using Unite.Composer.Web.Models.Identity.Validators;
using Unite.Composer.Web.Services.Validation;
using Unite.Identity.Entities;
using Unite.Identity.Services;
using Unite.Identity.Services.Configuration.Options;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddValidation();

            services.AddScoped<IdentityDbContext>();

            services.AddTransient<IAccessibilityService, AccessibilityService>();
            services.AddTransient<IIdentityService<User>, IdentityService>();
            services.AddTransient<ISessionService<User, UserSession>, SessionService>();

            services.AddTransient<IDonorsSearchService, DonorsSearchService>();
            services.AddTransient<ISpecimensSearchService, SpecimensSearchService>();
            services.AddTransient<IGenesSearchService, GenesSearchService>();
            services.AddTransient<IMutationsSearchService, MutationsSearchService>();

            services.AddTransient<OncoGridDataService>();
            services.AddTransient<OncoGridDataService1>();
            services.AddTransient<LolliplotDataService>();
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
