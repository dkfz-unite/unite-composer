using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Context.Repositories;

namespace Unite.Composer.Download.Repositories;

public abstract class DataRepository
{
    protected readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    protected readonly DonorsRepository _donorsRepository;
    protected readonly ImagesRepository _imagesRepository;
    protected readonly SpecimensRepository _specimensRepository;
    protected readonly GenesRepository _genesRepository;
    protected readonly VariantsRepository _variantsRepository;


    public DataRepository(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _donorsRepository = new DonorsRepository(dbContextFactory);
        _imagesRepository = new ImagesRepository(dbContextFactory);
        _specimensRepository = new SpecimensRepository(dbContextFactory);
        _genesRepository = new GenesRepository(dbContextFactory);
        _variantsRepository = new VariantsRepository(dbContextFactory);
    }
}
