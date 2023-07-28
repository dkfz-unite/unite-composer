using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;

namespace Unite.Composer.Download.Tsv;

public class GenesTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public GenesTsvDownloadService(
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
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDonorsDataForGenes(ids));
            }
            
            if (dataTypes.Clinical == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForGenes(ids));
            }

            if (dataTypes.Treatments == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForGenes(ids));
            }

            if (dataTypes.Mris == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetMriImagesDataForGenes(ids));
            }

            if (dataTypes.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Tissues, _specimensTsvService.GetTissuesDataForGenes(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Cells, _specimensTsvService.GetCellLinesDataForGenes(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetOrganoidsDataForGenes(ids));

                await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetXenograftsDataForGenes(ids));
            }

            if (dataTypes.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetOrganoidInterventionsDataForGenes(ids));

                await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetXenograftInterventionsDataForGenes(ids));
            }

            if (dataTypes.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.CellsDrugs, _specimensTsvService.GetCellLineDrugScreeningsDataForGenes(ids));

                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetOrganoidDrugScreeningsDataForGenes(ids));

                await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetXenograftDrugScreeningsDataForGenes(ids));
            }

            if (dataTypes.Ssms == true)
            {
                if (dataTypes.SsmsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullSsmsDataForGenes(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetSsmsDataForGenes(ids, dataTypes.SsmsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Cnvs == true)
            {
                if (dataTypes.CnvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullCnvsDataForGenes(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetCnvsDataForGenes(ids, dataTypes.CnvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Svs == true)
            {
                if (dataTypes.SvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullSvsDataForGenes(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetSvsDataForGenes(ids, dataTypes.SvsTranscriptsSlim ?? false));
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
