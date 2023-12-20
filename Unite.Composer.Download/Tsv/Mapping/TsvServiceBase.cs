using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public abstract class TsvServiceBase
{
    protected readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    protected readonly DonorsRepository _donorsRepository;
    protected readonly ImagesRepository _imagesRepository;
    protected readonly SpecimensRepository _specimensRepository;
    protected readonly GenesRepository _genesRepository;
    protected readonly VariantsRepository _variantsRepository;

    public TsvServiceBase(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _donorsRepository = new DonorsRepository(dbContextFactory);
        _imagesRepository = new ImagesRepository(dbContextFactory);
        _specimensRepository = new SpecimensRepository(dbContextFactory);
        _genesRepository = new GenesRepository(dbContextFactory);
        _variantsRepository = new VariantsRepository(dbContextFactory);
    }

    protected static string Write<T>(IEnumerable<T> items, ClassMap<T> map)
        where T : class
    {
        return items?.Any() == true ? TsvWriter.Write(items, map) : null;
    }
}
