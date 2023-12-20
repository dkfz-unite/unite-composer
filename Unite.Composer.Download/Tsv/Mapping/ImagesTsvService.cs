using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Context;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Images.Enums;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class ImagesTsvService : TsvServiceBase
{
    public ImagesTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public async Task<string> GetData(IEnumerable<int> ids, ImageType typeId)
    {
        if (typeId == ImageType.MRI)
            return await GetMriImagesData(ids);
        
        return null;
    }

    public async Task<string> GetDataForDonors(IEnumerable<int> ids, ImageType typeId)
    {
        var imageIds = await _donorsRepository.GetRelatedImages(ids, typeId);

        return await GetData(imageIds, typeId);
    }

    public async Task<string> GetDataForSpecimens(IEnumerable<int> ids, ImageType typeId)
    {
        var imageIds = await _specimensRepository.GetRelatedImages(ids, typeId);

        return await GetData(imageIds, typeId);
    }

    public async Task<string> GetDataForGenes(IEnumerable<int> ids, ImageType typeId)
    {
        var imageIds = await _genesRepository.GetRelatedImages(ids, typeId);

        return await GetData(imageIds, typeId);
    }

    public async Task<string> GetDataForVariants<TV>(IEnumerable<long> ids, ImageType typeId)
        where TV : Variant
    {
        var imageIds = await _variantsRepository.GetRelatedImages<TV>(ids, typeId);

        return await GetData(imageIds, typeId);
    }


    private async Task<string> GetMriImagesData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateMriImagesQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        var map = new ClassMap<Image>().MapMriImages();

        return Write(entities, map);
    }

    private static IQueryable<Image> CreateMriImagesQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Image>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MriImage);
    }
}
