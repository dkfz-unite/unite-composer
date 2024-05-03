using Microsoft.AspNetCore.Mvc;

namespace Unite.Composer.Web.Configuration.Attributes;

public class CompressResponseAttribute : MiddlewareFilterAttribute
{
    public CompressResponseAttribute () : base(typeof(CompressResponseAttribute ))
    {
    }

    public void Configure(IApplicationBuilder applicationBuilder) 
    {
        applicationBuilder.UseResponseCompression();
    }
}
