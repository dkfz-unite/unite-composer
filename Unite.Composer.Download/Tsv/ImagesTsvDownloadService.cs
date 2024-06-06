using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Genome.Analysis.Dna.Enums;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;

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


    public async Task<byte[]> Download(int id, ImageType type, DataTypesCriteria criteria)
    {
        var ids = new[] { id };

        return await Download(ids, type, criteria);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, ImageType type, DataTypesCriteria criteria)
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (criteria.Donors == true)
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDataForImages(ids));
            
            if (criteria.Clinical == true)
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForImages(ids));

            if (criteria.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForImages(ids));

            if (type == ImageType.MRI)
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetData(ids, ImageType.MRI));

            if (criteria.Specimens == true)
                await CreateArchiveEntry(archive, TsvFileNames.Materials, _specimensTsvService.GetDataForImages(ids, SpecimenType.Material));

            if (criteria.Ssms == true)
            {
                if (criteria.SsmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullDataForImages(ids, VariantType.SSM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetDataForImages(ids, VariantType.SSM, criteria.SsmsTranscriptsSlim ?? false));
            }

            if (criteria.Cnvs == true)
            {
                if (criteria.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullDataForImages(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetDataForImages(ids, VariantType.CNV, criteria.CnvsTranscriptsSlim ?? false));
            }

            if (criteria.Svs == true)
            {
                if (criteria.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullDataForImages(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetDataForImages(ids, VariantType.SV, criteria.SvsTranscriptsSlim ?? false));
            }

            if (criteria.GeneExp == true)
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetDataForImages(ids));
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
