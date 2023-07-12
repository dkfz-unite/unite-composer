using System.IO.Compression;
using Unite.Composer.Download;
using Unite.Composer.Download.Models;
using Unite.Composer.Web.Services.Download.Tsv.Constants;
using Unite.Data.Entities.Specimens.Enums;

namespace Unite.Composer.Web.Services.Download.Tsv;

public class SpecimensTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public SpecimensTsvDownloadService(
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


    public async Task<byte[]> Download(int id, SpecimenType type, DataTypes dataTypes)
    {
        var ids = new[] { id };

        return await Download(ids, type, dataTypes);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, SpecimenType type, DataTypes dataTypes)
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (dataTypes.Donors == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDonorsDataForSpecimens(ids));
            }
            
            if (dataTypes.Clinical == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForSpecimens(ids));
            }

            if (dataTypes.Treatments == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForSpecimens(ids));
            }

            if (dataTypes.Mris == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetMriImagesDataForSpecimens(ids));
            }

            if (dataTypes.Cts == true)
            {
                // await CreateArchiveEntry(archive, TsvFileNames.Cts, _imagesTsvService.GetCtImagesDataForSpecimens(ids));
            }

            if (dataTypes.Specimens == true)
            {
                if (type == SpecimenType.Tissue)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Tissues, _specimensTsvService.GetTissuesData(ids));
                }

                if (type == SpecimenType.CellLine)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cells, _specimensTsvService.GetCellLinesData(ids));
                }

                if (type == SpecimenType.Organoid)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetOrganoidsData(ids));
                }

                if (type == SpecimenType.Xenograft)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetXenograftsData(ids));
                }
            }

            if (dataTypes.Interventions == true)
            {
                if (type == SpecimenType.Organoid)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetOrganoidInterventionsData(ids));
                }

                if (type == SpecimenType.Xenograft)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetXenograftInterventionsData(ids));
                }
            }

            if (dataTypes.Drugs == true)
            {
                if (type == SpecimenType.CellLine)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.CellsDrugs, _specimensTsvService.GetCellLineDrugScreeningsData(ids));
                }

                if (type == SpecimenType.Organoid)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetOrganoidDrugScreeningsData(ids));
                }

                if (type == SpecimenType.Xenograft)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetXenograftDrugScreeningsData(ids));
                }
            }

            if (dataTypes.Ssms == true)
            {
                if (dataTypes.SsmsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullSsmsData(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetSsmsData(ids, dataTypes.SsmsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Cnvs == true)
            {
                if (dataTypes.CnvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullCnvsData(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetCnvsData(ids, dataTypes.CnvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Svs == true)
            {
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
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetTranscriptomicsData(ids));
            }
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
