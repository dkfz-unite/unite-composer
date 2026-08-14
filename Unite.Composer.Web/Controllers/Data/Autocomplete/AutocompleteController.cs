using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Composer.Data.Autocomplete;
using Unite.Data.Context;

namespace Unite.Composer.Web.Controllers.Data.Autocomplete;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AutocompleteController : Controller
{
    private readonly AutocompleteService _autocompleteService;


    public AutocompleteController(DomainDbContext dbContext)
    {
        _autocompleteService = new AutocompleteService(dbContext);
    }

    //TODO: Find the way to make this as generic as possible
    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string model, [FromQuery] string field, [FromQuery] string query)
    {
        if (string.IsNullOrEmpty(model))
            return BadRequest("Model is required.");

        if (string.IsNullOrEmpty(field))
            return BadRequest("Field is required.");

        var comparison = StringComparison.InvariantCultureIgnoreCase;

        string[] results = null;

        if (model.Equals("gene", comparison))
        {
            if (field.Equals("symbol", comparison))
                results = await _autocompleteService.Search<Unite.Data.Entities.Omics.Gene>(gene => gene.Symbol, query);
        }
        else if (model.Equals("protein", comparison))
        {
            if (field.Equals("symbol", comparison))
                results = await _autocompleteService.Search<Unite.Data.Entities.Omics.Protein>(protein => protein.Symbol, query);
        }

        return Ok(results);   
    } 
}
