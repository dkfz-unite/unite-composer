using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download.Tsv.Mapping;

public class ImagesTsvService : TsvServiceBase
{
    //TODO: Use DbContextFactory per request to allow parallel queries
    private readonly DomainDbContext _dbContext;


    public ImagesTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }


    public async Task<string> GetMriImagesData(IEnumerable<int> ids)
    {
        var entities = await CreateMriImagesQuery()
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Image>().MapMriImages();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetMriImagesDataForDonors(IEnumerable<int> ids)
    {
        var imageIds = await GetImageIdsForDonors(ids);

        return await GetMriImagesData(imageIds);
    }

    public async Task<string> GetMriImagesDataForSpecimens(IEnumerable<int> ids)
    {
        var imageIds = await GetImageIdsForSpecimens(ids);

        return await GetMriImagesData(imageIds);
    }

    public async Task<string> GetMriImagesDataForGenes(IEnumerable<int> ids)
    {
        var imageIds = await GetImageIdsForGenes(ids);

        return await GetMriImagesData(imageIds);
    }

    public async Task<string> GetMriImagesDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var imageIds = await GetImageIdsForVariants<TVO, TV>(ids);

        return await GetMriImagesData(imageIds);
    }


    private async Task<int[]> GetImageIdsForDonors(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Image>().AsNoTracking()
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetImageIdsForSpecimens(IEnumerable<int> ids)
    {
        var donorIds = await _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Tissue)
            .Where(entity => entity.Tissue != null && entity.Tissue.TypeId != TissueType.Control)
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();

        return await GetImageIdsForDonors(donorIds);
    }

    private async Task<int[]> GetImageIdsForGenes(IEnumerable<int> ids)
    {
        var donorIds = await GetDonorIdsForGenes(ids);

        return await GetImageIdsForDonors(donorIds);
    }

    private async Task<int[]> GetImageIdsForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var donorIds = await GetDonorIdsForVariants<TVO, TV>(ids);

        return await GetImageIdsForDonors(donorIds);
    }


    private IQueryable<Image> CreateMriImagesQuery()
    {
        return _dbContext.Set<Image>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MriImage);
    }
}
