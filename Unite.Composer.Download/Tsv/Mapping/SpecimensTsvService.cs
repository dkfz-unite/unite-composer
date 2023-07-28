using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Tsv.Mapping.Extensions;
using Unite.Data.Entities.Images;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Specimens;
using Unite.Data.Entities.Specimens.Tissues.Enums;
using Unite.Data.Services;
using Unite.Essentials.Tsv;

using OrganoidIntervention = Unite.Data.Entities.Specimens.Organoids.Intervention;
using XenograftIntervention = Unite.Data.Entities.Specimens.Xenografts.Intervention;

namespace Unite.Composer.Download.Tsv.Mapping;

public class SpecimensTsvService : TsvServiceBase
{
    //TODO: Use DbContextFactory per request to allow parallel queries
    private readonly DomainDbContext _dbContext;


    public SpecimensTsvService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
        _dbContext = dbContextFactory.CreateDbContext();
    }
    

    public async Task<string> GetTissuesData(IEnumerable<int> ids)
    {
        using var dbContext = _dbContextFactory.CreateDbContext();
        
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.Tissue.Source)
            .Where(entity => entity.Tissue != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapTissues();
        
        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetTissuesDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetTissuesData(specimenIds);
    }

    public async Task<string> GetTissuesDataForImages(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForImages(ids);

        return await GetTissuesData(specimenIds);
    }

    public async Task<string> GetTissuesDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetTissuesData(specimenIds);
    }

    public async Task<string> GetTissuesDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetTissuesData(specimenIds);
    }


    public async Task<string> GetCellLinesData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.CellLine.Info)
            .Where(entity => entity.CellLine != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapCellLines();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetCellLinesDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetCellLinesData(specimenIds);
    }

    public async Task<string> GetCellLinesDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetCellLinesData(specimenIds);
    }

    public async Task<string> GetCellLinesDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetCellLinesData(specimenIds);
    }


    public async Task<string> GetOrganoidsData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.Organoid)
            .Where(entity => entity.Organoid != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapOrganoids();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetOrganoidsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetOrganoidsData(specimenIds);
    }

    public async Task<string> GetOrganoidsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetOrganoidsData(specimenIds);
    }

    public async Task<string> GetOrganoidsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetOrganoidsData(specimenIds);
    }


    public async Task<string> GetXenograftsData(IEnumerable<int> ids)
    {
        var entities = await CreateSpecimensQuery()
            .Include(entity => entity.Xenograft)
            .Where(entity => entity.Xenograft != null)
            .Where(entity => ids.Contains(entity.Id))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<Specimen>().MapXenografts();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetXenograftsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetXenograftsData(specimenIds);
    }

    public async Task<string> GetXenograftsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetXenograftsData(specimenIds);
    }

    public async Task<string> GetXenograftsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetXenograftsData(specimenIds);
    }


    public async Task<string> GetOrganoidInterventionsData(IEnumerable<int> ids)
    {
        var entities = await CreateOrganoidInterventionsQuery()
            .Where(entity => ids.Contains(entity.Organoid.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<OrganoidIntervention>().MapInterventions();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetOrganoidInterventionsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetOrganoidInterventionsData(specimenIds);
    }

    public async Task<string> GetOrganoidInterventionsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetOrganoidInterventionsData(specimenIds);
    }

    public async Task<string> GetOrganoidInterventionsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetOrganoidInterventionsData(specimenIds);
    }


    public async Task<string> GetXenograftInterventionsData(IEnumerable<int> ids)
    {
        var entities = await CreateXenograftInterventionsQuery()
            .Where(entity => ids.Contains(entity.Xenograft.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<XenograftIntervention>().MapInterventions();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetXenograftInterventionsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetXenograftInterventionsData(specimenIds);
    }

    public async Task<string> GetXenograftInterventionsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetXenograftInterventionsData(specimenIds);
    }

    public async Task<string> GetXenograftInterventionsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetXenograftInterventionsData(specimenIds);
    }


    public async Task<string> GetCellLineDrugScreeningsData(IEnumerable<int> ids)
    {
        var entities = await CreateDrugScreeningsQuery()
            .Where(entity => entity.Specimen.CellLine != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetCellLineDrugScreeningsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetCellLineDrugScreeningsData(specimenIds);
    }

    public async Task<string> GetCellLineDrugScreeningsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetCellLineDrugScreeningsData(specimenIds);
    }

    public async Task<string> GetCellLineDrugScreeningsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetCellLineDrugScreeningsData(specimenIds);
    }


    public async Task<string> GetOrganoidDrugScreeningsData(IEnumerable<int> ids)
    {
        var entities = await CreateDrugScreeningsQuery()
            .Where(entity => entity.Specimen.Organoid != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetOrganoidDrugScreeningsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetOrganoidDrugScreeningsData(specimenIds);
    }

    public async Task<string> GetOrganoidDrugScreeningsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetOrganoidDrugScreeningsData(specimenIds);
    }

    public async Task<string> GetOrganoidDrugScreeningsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetOrganoidDrugScreeningsData(specimenIds);
    }


    public async Task<string> GetXenograftDrugScreeningsData(IEnumerable<int> ids)
    {
        var entities = await CreateDrugScreeningsQuery()
            .Where(entity => entity.Specimen.Xenograft != null)
            .Where(entity => ids.Contains(entity.SpecimenId))
            .ToArrayAsync();

        if (!entities.Any())
        {
            return null;
        }

        var map = new ClassMap<DrugScreening>().MapDrugScreenings();

        return TsvWriter.Write(entities, map);
    }

    public async Task<string> GetXenograftDrugScreeningsDataForDonors(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForDonors(ids);

        return await GetXenograftDrugScreeningsData(specimenIds);
    }

    public async Task<string> GetXenograftDrugScreeningsDataForGenes(IEnumerable<int> ids)
    {
        var specimenIds = await GetSpecimenIdsForGenes(ids);

        return await GetXenograftDrugScreeningsData(specimenIds);
    }

    public async Task<string> GetXenograftDrugScreeningsDataForVariants<TVO, TV>(IEnumerable<long> ids)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var specimenIds = await GetSpecimenIdsForVariants<TVO, TV>(ids);

        return await GetXenograftDrugScreeningsData(specimenIds);
    }


    private async Task<int[]> GetSpecimenIdsForDonors(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }

    private async Task<int[]> GetSpecimenIdsForImages(IEnumerable<int> ids)
    {
        var donorIds = await _dbContext.Set<Image>().AsNoTracking()
            .Where(entity => ids.Contains(entity.Id))
            .Select(entity => entity.DonorId)
            .Distinct()
            .ToArrayAsync();

        return await _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Tissue)
            .Where(entity => entity.Tissue != null && entity.Tissue.TypeId != TissueType.Control)
            .Where(entity => ids.Contains(entity.DonorId))
            .Select(entity => entity.Id)
            .Distinct()
            .ToArrayAsync();
    }


    private IQueryable<Specimen> CreateSpecimensQuery()
    {
        return _dbContext.Set<Specimen>().AsNoTracking()
            .Include(entity => entity.Donor)
            .Include(entity => entity.MolecularData)
            .Include(entity => entity.Parent.Tissue)
            .Include(entity => entity.Parent.CellLine)
            .Include(entity => entity.Parent.Organoid)
            .Include(entity => entity.Parent.Xenograft);
    }

    private IQueryable<OrganoidIntervention> CreateOrganoidInterventionsQuery()
    {
        return _dbContext.Set<OrganoidIntervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Organoid.Specimen.Donor)
            .Include(entity => entity.Organoid.Specimen.Organoid);
    }

    private IQueryable<XenograftIntervention> CreateXenograftInterventionsQuery()
    {
        return _dbContext.Set<XenograftIntervention>().AsNoTracking()
            .Include(entity => entity.Type)
            .Include(entity => entity.Xenograft.Specimen.Donor)
            .Include(entity => entity.Xenograft.Specimen.Xenograft);
    }

    private IQueryable<DrugScreening> CreateDrugScreeningsQuery()
    {
        return _dbContext.Set<DrugScreening>().AsNoTracking()
            .Include(entity => entity.Drug)
            .Include(entity => entity.Specimen.Donor)
            .Include(entity => entity.Specimen.Tissue)
            .Include(entity => entity.Specimen.CellLine)
            .Include(entity => entity.Specimen.Organoid)
            .Include(entity => entity.Specimen.Xenograft);
    }
}
