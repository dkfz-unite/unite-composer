using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unite.Composer.Web.Configuration.Extensions;

namespace Unite.Composer.Web
{
    public class Startup
    {
        private readonly string[] _allowedDomains = {
            "https://*.unite.net",
            "http://*.unite.net",
            "https://*.localhost",
            "http://*.localhost",
            "http://*.localhost:8080",
            "https://*.localhost:8081"
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication("Bearer").AddCookie("Bearer", options =>
            //{
            //    options.Cookie.Name = "unite_identity";
            //});

            services.AddServices();

            services.AddCors();

            services.AddAuthentication(options => options.AddJwtAuthenticationOptions())
                    .AddJwtBearer(options => options.AddJwtBearerOptions());

            services.AddControllers(options => options.AddMvcOptions())
                    .AddJsonOptions(options => options.AddJsonOptions())
                    .AddFluentValidation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseRouting();

            //app.UseCors(builer =>
            //{
            //    //builer.AllowAnyOrigin()
            //    //builer.WithOrigins(_allowedDomains).SetIsOriginAllowedToAllowWildcardSubdomains()
            //          .AllowAnyMethod()
            //          .AllowAnyHeader()

            //          .AllowCredentials();
            //});

            app.UseCors(builder => builder
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
            );

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
