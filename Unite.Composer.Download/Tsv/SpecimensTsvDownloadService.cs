using System.IO.Compression;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Composer.Download.Tsv.Models;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Omics.Analysis.Dna.Enums;
using Unite.Data.Entities.Specimens.Enums;

namespace Unite.Composer.Download.Tsv;

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


    public async Task<byte[]> Download(int id, SpecimenType type, DataTypesCriteria criteria)
    {
        var ids = new[] { id };

        return await Download(ids, type, criteria);
    }

    public async Task<byte[]> Download(IEnumerable<int> ids, SpecimenType type, DataTypesCriteria criteria)
    {
        var archiveBytes = Array.Empty<byte>();
        using (var archiveStream = new MemoryStream())
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, true))
        {
            if (criteria.Donors == true)
                await CreateArchiveEntry(archive, TsvFileNames.Donor, _donorsTsvService.GetDataForSpecimens(ids));
            
            // if (criteria.Clinical == true)
            //     await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForSpecimens(ids));

            if (criteria.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatment, _donorsTsvService.GetTreatmentsDataForSpecimens(ids));

            if (criteria.Mrs == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mr, _imagesTsvService.GetDataForSpecimens(ids, ImageType.MR));

            if (criteria.Specimens == true)
            {
                if (type == SpecimenType.Material)
                    await CreateArchiveEntry(archive, TsvFileNames.Material, _specimensTsvService.GetData(ids, SpecimenType.Material));

                if (type == SpecimenType.Line)
                    await CreateArchiveEntry(archive, TsvFileNames.Line, _specimensTsvService.GetData(ids, SpecimenType.Line));

                if (type == SpecimenType.Organoid)
                    await CreateArchiveEntry(archive, TsvFileNames.Organoid, _specimensTsvService.GetData(ids, SpecimenType.Organoid));

                if (type == SpecimenType.Xenograft)
                    await CreateArchiveEntry(archive, TsvFileNames.Xenograft, _specimensTsvService.GetData(ids, SpecimenType.Xenograft));
            }

            if (criteria.Interventions == true)
            {
                if (type == SpecimenType.Line)
                    await CreateArchiveEntry(archive, TsvFileNames.LineIntervention, _specimensTsvService.GetInterventionsData(ids, SpecimenType.Line));

                if (type == SpecimenType.Organoid)
                    await CreateArchiveEntry(archive, TsvFileNames.OrganoidIntervention, _specimensTsvService.GetInterventionsData(ids, SpecimenType.Organoid));

                if (type == SpecimenType.Xenograft)
                    await CreateArchiveEntry(archive, TsvFileNames.XenograftIntervention, _specimensTsvService.GetInterventionsData(ids, SpecimenType.Xenograft));
            }

            if (criteria.Drugs == true)
            {
                if (type == SpecimenType.Line)
                    await CreateArchiveEntry(archive, TsvFileNames.LineDrug, _specimensTsvService.GetDrugsScreeningsData(ids, SpecimenType.Line));

                if (type == SpecimenType.Organoid)
                    await CreateArchiveEntry(archive, TsvFileNames.OrganoidDrug, _specimensTsvService.GetDrugsScreeningsData(ids, SpecimenType.Organoid));

                if (type == SpecimenType.Xenograft)
                    await CreateArchiveEntry(archive, TsvFileNames.XenograftDrug, _specimensTsvService.GetDrugsScreeningsData(ids, SpecimenType.Xenograft));
            }

            if (criteria.Sms == true)
            {
                if (criteria.SmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Sm, _variantsTsvService.GetFullDataForSpecimens(ids, VariantType.SM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Sm, _variantsTsvService.GetDataForSpecimens(ids, VariantType.SM, criteria.SmsTranscriptsSlim ?? false));
            }

            if (criteria.Cnvs == true)
            {
                if (criteria.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnv, _variantsTsvService.GetFullDataForSpecimens(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnv, _variantsTsvService.GetDataForSpecimens(ids, VariantType.CNV, criteria.CnvsTranscriptsSlim ?? false));
            }

            if (criteria.Svs == true)
            {
                if (criteria.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Sv, _variantsTsvService.GetFullDataForSpecimens(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Sv, _variantsTsvService.GetDataForSpecimens(ids, VariantType.SV, criteria.SvsTranscriptsSlim ?? false));
            }

            if (criteria.GeneExp == true)
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetDataForSpecimens(ids));
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
