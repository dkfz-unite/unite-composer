using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Images.Enums;

namespace Unite.Composer.Download.Tsv;

public class ImagesTsvDownloadService : TsvDownloadService
{
    private readonly DonorsTsvService _donorsTsvService;
    private readonly ImagesTsvService _imagesTsvService;
    private readonly SpecimensTsvService _specimensTsvService;
    private readonly VariantsTsvService _variantsTsvService;
    private readonly TranscriptomicsTsvService _transcriptomicsTsvService;


    public ImagesTsvDownloadService(
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


    public async Task<byte[]> Download(int id, ImageType type, DataTypes dataTypes)
    {
        var ids = new[] { id };

        return await Download(ids, type, dataTypes);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, ImageType type, DataTypes dataTypes)
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (dataTypes.Donors == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDonorsDataForImages(ids));
            }
            
            if (dataTypes.Clinical == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForImages(ids));
            }

            if (dataTypes.Treatments == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForImages(ids));
            }

            if (type == ImageType.MRI)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetMriImagesData(ids));
            }

            if (type == ImageType.CT)
            {
                // await CreateArchiveEntry(archive, "TsvFileNames.Cts", _imagesTsvService.GetCtImagesData(ids));
            }

            if (dataTypes.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Tissues, _specimensTsvService.GetTissuesDataForImages(ids));
            }

            if (dataTypes.Ssms == true)
            {
                if (dataTypes.SsmsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullSsmsDataForImages(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetSsmsDataForImages(ids, dataTypes.SsmsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Cnvs == true)
            {
                if (dataTypes.CnvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullCnvsDataForImages(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetCnvsDataForImages(ids, dataTypes.CnvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.Svs == true)
            {
                if (dataTypes.SvsTranscriptsFull == true)
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullSvsDataForImages(ids));
                }
                else
                {
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetSvsDataForImages(ids, dataTypes.SvsTranscriptsSlim ?? false));
                }
            }

            if (dataTypes.GeneExp == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetTranscriptomicsDataForImages(ids));
            }
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
