﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Unite.Composer.Admin.Services;
using Unite.Composer.Identity.Services;
using Unite.Composer.Search.Services;
using Unite.Composer.Visualization.Lolliplot;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Ensembl.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Uniprot.Configuration.Options;
using Unite.Composer.Visualization.Oncogrid;
using Unite.Composer.Web.Configuration.Options;
using Unite.Composer.Web.HostedServices;
using Unite.Composer.Web.Models.Admin;
using Unite.Composer.Web.Models.Admin.Validators;
using Unite.Composer.Web.Models.Identity;
using Unite.Composer.Web.Models.Identity.Validators;
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

            services.AddTransient<IdentityDbContext>();

            services.AddTransient<UserService>();
            services.AddTransient<IdentityService>();
            services.AddTransient<SessionService>();

            services.AddTransient<IDonorsSearchService, DonorsSearchService>();
            services.AddTransient<ISpecimensSearchService, SpecimensSearchService>();
            services.AddTransient<IGenesSearchService, GenesSearchService>();
            services.AddTransient<IMutationsSearchService, MutationsSearchService>();
            services.AddTransient<IImagesSearchService, ImagesSearchService>();

            services.AddTransient<OncoGridDataService>();
            services.AddTransient<OncoGridDataService1>();
            services.AddTransient<ProteinPlotDataService>();

            services.AddHostedService<RootHostedService>();
        }

        private static void AddOptions(this IServiceCollection services)
        {
            services.AddTransient<ApiOptions>();
            services.AddTransient<RootOptions>();
            services.AddTransient<IElasticOptions, ElasticOptions>();
            services.AddTransient<ISqlOptions, SqlOptions>();
            services.AddTransient<IEnsemblOptions, EnsemblOptions>();
            services.AddTransient<IUniprotOptions, UniprotOptions>();
            services.AddTransient<IPfamOptions, PfamOptions>();
        }

        private static void AddValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<AddUserModel>, AddUserModelValidator>();
            services.AddTransient<IValidator<EditUserModel>, EditUserModelValidator>();
            services.AddTransient<IValidator<SignUpModel>, SignUpModelValidator>();
            services.AddTransient<IValidator<SignInModel>, SignInModelValidator>();
            services.AddTransient<IValidator<PasswordChangeModel>, PasswordChangeModelValidator>();
        }
    }
}
