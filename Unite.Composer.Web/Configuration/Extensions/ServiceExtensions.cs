using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Identity.Models;
using Unite.Composer.Identity.Models.Validation;
using Unite.Composer.Identity.Services;
using Unite.Composer.Indices.Services;
using Unite.Composer.Validation;
using Unite.Composer.Web.Configuration.Options;
using Unite.Data.Entities.Identity;
using Unite.Data.Services;
using Unite.Data.Services.Configuration.Options;
using Unite.Indices.Services.Configuration.Options;

namespace Unite.Composer.Web.Configuration.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddOptions();
            services.AddValidation();

            services.AddScoped<UniteDbContext>();

            services.AddTransient<IAccessibilityService, AccessibilityService>();
            services.AddTransient<IIdentityService<User>, IdentityService>();
            services.AddTransient<ISessionService<User, UserSession>, SessionService>();
            services.AddTransient<IIndexService<Unite.Indices.Entities.Donors.DonorIndex>, DonorIndexService>();
            services.AddTransient<IIndexService<Unite.Indices.Entities.Mutations.MutationIndex>, MutationIndexService>();
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
