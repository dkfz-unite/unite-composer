using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Genome.Variants;
using Unite.Data.Entities.Genome.Variants.Enums;

using SSM = Unite.Data.Entities.Genome.Variants.SSM;
using CNV = Unite.Data.Entities.Genome.Variants.CNV;
using SV = Unite.Data.Entities.Genome.Variants.SV;

namespace Unite.Composer.Download.Tsv;

public class VariantsTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public VariantsTsvDownloadService(
        DonorsTsvService donorsTsvService,
        ImagesTsvService imagesTsvService,
        SpecimensTsvService specimensTsvService,
        VariantsTsvService variantsTsvService,
        TranscriptomicsTsvService transcriptomicsTsvService)
    {
        _donorsTsvService = donorsTsvService;
        _imagesTsvService = imagesTsvService;
        _specimensTsvService = specimensTsvService;
        _variantsTsvService = variantsTsvService;
        _transcriptomicsTsvService = transcriptomicsTsvService;
    }


    public async Task<byte[]> Download(long id, VariantType type, DataTypes dataTypes)
    {
        var ids = new[] { id };

        return await Download(ids, type, dataTypes);
    }

    public async Task<byte[]> Download(IEnumerable<long> ids, VariantType type, DataTypes dataTypes)
    {
        if (type == VariantType.SSM)
            return await Download<SSM.VariantOccurrence, SSM.Variant>(ids, type, dataTypes);
        else if (type == VariantType.CNV)
            return await Download<CNV.VariantOccurrence, CNV.Variant>(ids, type, dataTypes);
        else if (type == VariantType.SV)
            return await Download<SV.VariantOccurrence, SV.Variant>(ids, type, dataTypes);
        else
            throw new ArgumentException($"Unknown variant type: {type}");
    }


    private async Task<byte[]> Download<TVO, TV>(IEnumerable<long> ids, VariantType type, DataTypes dataTypes)
        where TVO : VariantOccurrence<TV>
        where TV : Variant
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (dataTypes.Donors == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDonorsForVariants<TVO, TV>(ids));
            }
            
            if (dataTypes.Clinical == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Treatments == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Mris == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetMriImagesDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Cts == true)
            {
                // await CreateArchiveEntry(archive, TsvFileNames.Cts, _imagesTsvService.GetCtImagesDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Tissues, _specimensTsvService.GetTissuesDataForVariants<TVO, TV>(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Cells, _specimensTsvService.GetCellLinesDataForVariants<TVO, TV>(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetOrganoidsDataForVariants<TVO, TV>(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetXenograftsDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetOrganoidInterventionsDataForVariants<TVO, TV>(ids));

                await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetXenograftInterventionsDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.CellsDrugs, _specimensTsvService.GetCellLineDrugScreeningsDataForVariants<TVO, TV>(ids));

                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetOrganoidDrugScreeningsDataForVariants<TVO, TV>(ids));

                await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetXenograftDrugScreeningsDataForVariants<TVO, TV>(ids));
            }

            if (dataTypes.Ssms == true && type == VariantType.SSM)
            {
                // TODO: Find intersecting variants
                if (dataTypes.SsmsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullSsmsData(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetSsmsData(ids, dataTypes.SsmsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Cnvs == true && type == VariantType.CNV)
            {
                // TODO: Find intersecting variants
                if (dataTypes.CnvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullCnvsData(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetCnvsData(ids, dataTypes.CnvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Svs == true && type == VariantType.SV)
            {
                // TODO: Find intersecting variants
                if (dataTypes.SvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullSvsData(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetSvsData(ids, dataTypes.SvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.GeneExp == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetTranscriptomicsDataForVariants<TVO, TV>(ids));
            }
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
