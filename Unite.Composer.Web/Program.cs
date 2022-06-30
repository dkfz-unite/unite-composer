using System.Security.Claims;
using FluentValidation.AspNetCore;
using Unite.Composer.Web.Configuration.Extensions;


var builder = WebApplication.CreateBuilder(args);


builder.Logging.ClearProviders();

builder.Logging.AddConsole();


builder.Services.AddServices();

builder.Services.AddCors();

builder.Services.AddAuthentication(options => options.AddJwtAuthenticationOptions())
                .AddJwtBearer(options => options.AddJwtBearerOptions());

builder.Services.AddAuthorization(options => options.AddAuthorizationOptions());

builder.Services.AddControllers(options => options.AddMvcOptions())
                .AddJsonOptions(options => options.AddJsonOptions())
                .AddFluentValidation();


var app = builder.Build();

app.UseRouting();

app.UseCors(builder => builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


app.Run();
