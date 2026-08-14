using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;

namespace Unite.Composer.Data.Autocomplete;

public class AutocompleteService
{
    private readonly DomainDbContext _dbContext;


    public AutocompleteService(DomainDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<string[]> Search<T>(Expression<Func<T, string>> property, string term) where T : class
    {
        var pattern = term?.Trim();
        var query = _dbContext.Set<T>().AsNoTracking();

        // TODO: This patter might be too loose and cause full table scans.
        // For small tables this is not a problem. But for some properties we may allow only prefix %{pattern} search.
        if (!string.IsNullOrEmpty(pattern))
            query = query.Where(entity => EF.Functions.Like(EF.Property<string>(entity, GetPropertyName(property)), $"%{pattern}%"));

        return await query
            .Select(property)
            .Distinct()
            .OrderBy(entity => entity)
            .Take(10)
            .ToArrayAsync();
    }
    
    private static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> property) where T : class
    {
        if (property.Body is MemberExpression memberExpression)
        {
            return memberExpression.Member.Name;
        }

        throw new ArgumentException("Invalid property expression");
    }
}
