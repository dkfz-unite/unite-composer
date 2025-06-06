using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Context;
using Unite.Data.Entities.Omics.Analysis.Dna;
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
        if (typeId == ImageType.MR)
            return await GetMrImagesData(ids);
        
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

    public async Task<string> GetDataForVariants<TV>(IEnumerable<int> ids, ImageType typeId)
        where TV : Variant
    {
        var imageIds = await _variantsRepository.GetRelatedImages<TV>(ids, typeId);

        return await GetData(imageIds, typeId);
    }


    private async Task<string> GetMrImagesData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();

        var entities = await CreateMrImagesQuery(dbContext)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        var map = new ClassMap<Image>().MapMrImages();

        return Write(entities, map);
    }

    private static IQueryable<Image> CreateMrImagesQuery(DomainDbContext dbContext)
    {
        return dbContext.Set<Image>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MrImage);
    }
}
