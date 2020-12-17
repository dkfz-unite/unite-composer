using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unite.Composer.Web.Configuration.Extensions;

namespace Unite.Composer.Web
{
    public class Startup
    {
        private static readonly string[] _trustedOrigins =
        {
            "http://localhost:80",
            "https://localhost:443",
            "http://web.unite:80",
            "https://web.unite:443",
            "http://localhost:5000",
            "https://localhost:5001",
            "http://localhost:8080"
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServices();

            services.AddControllers(options => options.AddMvcOptions())
                    .AddJsonOptions(options => options.AddJsonOptions());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(builer =>
            {
                builer.WithOrigins(_trustedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
