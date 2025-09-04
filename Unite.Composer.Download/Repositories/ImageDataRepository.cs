using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Images;

namespace Unite.Composer.Download.Repositories;

public class ImageDataRepository : DataRepository
{
    public ImageDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<Image[]> GetImages(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateImagesQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();
    }

    public async Task<Image[]> GetImagesForDonors(IEnumerable<int> ids)
    {
        var imageIds = await _donorsRepository.GetRelatedImages(ids);

        return await GetImages(imageIds);
    }

    public async Task<Image[]> GetImagesForSpecimens(IEnumerable<int> ids)
    {
        var imageIds = await _specimensRepository.GetRelatedImages(ids);

        return await GetImages(imageIds);
    }


    private static IQueryable<Image> CreateImagesQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Image>()
            .AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MrImage);
    }
}
