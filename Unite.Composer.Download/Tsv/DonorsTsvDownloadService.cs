using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;

namespace Unite.Composer.Download.Tsv;

public class DonorsTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public DonorsTsvDownloadService(
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


    public async Task<byte[]> Download(int id, DataTypes dataTypes)
    {
        var ids = new[] { id };

        return await Download(ids, dataTypes);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, DataTypes dataTypes)
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (dataTypes.Donors == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDonorsData(ids));
            }
            
            if (dataTypes.Clinical == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalData(ids));
            }

            if (dataTypes.Treatments == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsData(ids));
            }

            if (dataTypes.Mris == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetMriImagesDataForDonors(ids));
            }

            if (dataTypes.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Tissues, _specimensTsvService.GetTissuesDataForDonors(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Cells, _specimensTsvService.GetCellLinesDataForDonors(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetOrganoidsDataForDonors(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetXenograftsDataForDonors(ids));
            }

            if (dataTypes.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetOrganoidInterventionsDataForDonors(ids));

                await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetXenograftInterventionsDataForDonors(ids));
            }

            if (dataTypes.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.CellsDrugs, _specimensTsvService.GetCellLineDrugScreeningsDataForDonors(ids));

                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetOrganoidDrugScreeningsDataForDonors(ids));

                await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetXenograftDrugScreeningsDataForDonors(ids));
            }

            if (dataTypes.Ssms == true)
            {
                if (dataTypes.SsmsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullSsmsDataForDonors(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetSsmsDataForDonors(ids, dataTypes.SsmsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Cnvs == true)
            {
                if (dataTypes.CnvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullCnvsDataForDonors(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetCnvsDataForDonors(ids, dataTypes.CnvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Svs == true)
            {
                if (dataTypes.SvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullSvsDataForDonors(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetSvsDataForDonors(ids, dataTypes.SvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.GeneExp == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetTranscriptomicsDataForDonors(ids));
            }
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
