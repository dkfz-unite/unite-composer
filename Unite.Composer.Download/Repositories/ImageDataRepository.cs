using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Images.Enums;

namespace Unite.Composer.Download.Repositories;

public class ImageDataRepository : DataRepository
{
    public ImageDataRepository(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }


    public async Task<Image[]> GetImages(IEnumerable<int> ids, ImageType type)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        return await CreateImagesQuery(dbContext, type)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();
    }

    public async Task<Image[]> GetImagesForDonors(IEnumerable<int> ids, ImageType type)
    {
        var imageIds = await _donorsRepository.GetRelatedImages(ids, type);

        return await GetImages(imageIds, type);
    }

    public async Task<Image[]> GetImagesForSpecimens(IEnumerable<int> ids, ImageType type)
    {
        var imageIds = await _specimensRepository.GetRelatedImages(ids, type);

        return await GetImages(imageIds, type);
    }


    private static IQueryable<Image> CreateImagesQuery(DomainDbContext dbContext, ImageType type)
    {
        var query = dbContext.Set<Image>().AsNoTracking();

        if (type == ImageType.MR)
            query = query.Include(entity => entity.MrImage);
        // else if (type == ImageType.CT)
        //     query = query.Include(entity => entity.CtImage);
       
        return query
            .Include(entity => entity.Donor)
            .Where(entity => entity.TypeId == type);
    }
}
