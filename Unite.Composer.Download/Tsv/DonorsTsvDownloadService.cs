using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Genome.Variants.Enums;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;

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


    public async Task<byte[]> Download(int id, DataTypesCriteria criteria)
    {
        var ids = new[] { id };

        return await Download(ids, criteria);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, DataTypesCriteria criteria)
    {
        var archiveBytes = Array.Empty<byte>();
 
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (criteria.Donors == true)
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetData(ids));
            
            if (criteria.Clinical == true)
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalData(ids));

            if (criteria.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsData(ids));

            if (criteria.Mris == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetDataForDonors(ids, ImageType.MRI));

            if (criteria.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Materials, _specimensTsvService.GetDataForDonors(ids, SpecimenType.Material));
                await CreateArchiveEntry(archive, TsvFileNames.Lines, _specimensTsvService.GetDataForDonors(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetDataForDonors(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetDataForDonors(ids, SpecimenType.Xenograft));
            }

            if (criteria.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetInterventionsDataForDonors(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetInterventionsDataForDonors(ids, SpecimenType.Xenograft));
            }

            if (criteria.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LinesDrugs, _specimensTsvService.GetDrugsScreeningsDataForDonors(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetDrugsScreeningsDataForDonors(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetDrugsScreeningsDataForDonors(ids, SpecimenType.Xenograft));
            }

            if (criteria.Ssms == true)
            {
                if (criteria.SsmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullDataForDonors(ids, VariantType.SSM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetDataForDonors(ids,VariantType.SSM, criteria.SsmsTranscriptsSlim ?? false));
            }

            if (criteria.Cnvs == true)
            {
                if (criteria.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullDataForDonors(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetDataForDonors(ids, VariantType.CNV, criteria.CnvsTranscriptsSlim ?? false));
            }

            if (criteria.Svs == true)
            {
                if (criteria.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullDataForDonors(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetDataForDonors(ids, VariantType.SV, criteria.SvsTranscriptsSlim ?? false));
            }

            if (criteria.GeneExp == true)
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetDataForDonors(ids));
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
