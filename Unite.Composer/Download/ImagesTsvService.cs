using Microsoft.EntityFrameworkCore;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

namespace Unite.Composer.Download;

public class ImagesTsvService
{
    private readonly DomainDbContext _dbContext;


    public ImagesTsvService(IDbContextFactory<DomainDbContext> dbContextFactory)
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

        var map = CreateMriImagesMap();

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


    private IQueryable<Image> CreateMriImagesQuery()
    {
        return _dbContext.Set<Image>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MriImage);
    }


    private static ClassMap<Image> CreateMriImagesMap()
    {
        return new ClassMap<Image>()
            .Map(entity => entity.Donor.ReferenceId, "donor_id")
            .Map(entity => entity.ReferenceId, "image_id")
            .Map(entity => entity.Type, "image_type")
            .Map(entity => entity.MriImage.WholeTumor, "whole_tumor")
            .Map(entity => entity.MriImage.ContrastEnhancing, "contrast_enhancing")
            .Map(entity => entity.MriImage.NonContrastEnhancing, "non_contrast_enhancing")
            .Map(entity => entity.MriImage.MedianAdcTumor, "median_adc_tumor")
            .Map(entity => entity.MriImage.MedianAdcCe, "median_adc_ce")
            .Map(entity => entity.MriImage.MedianAdcEdema, "median_adc_edema")
            .Map(entity => entity.MriImage.MedianCbfTumor, "median_cbf_tumor")
            .Map(entity => entity.MriImage.MedianCbfCe, "median_cbf_ce")
            .Map(entity => entity.MriImage.MedianCbfEdema, "median_cbf_edema")
            .Map(entity => entity.MriImage.MedianCbvTumor, "median_cbv_tumor")
            .Map(entity => entity.MriImage.MedianCbvCe, "median_cbv_ce")
            .Map(entity => entity.MriImage.MedianCbvEdema, "median_cbv_edema")
            .Map(entity => entity.MriImage.MedianMttTumor, "median_mtt_tumor")
            .Map(entity => entity.MriImage.MedianMttCe, "median_mtt_ce")
            .Map(entity => entity.MriImage.MedianMttEdema, "median_mtt_edema");
    }
}
