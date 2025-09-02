using System.IO.Compression;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Composer.Download.Tsv.Models;
using Unite.Data.Entities.Omics.Analysis.Dna.Enums;
using Unite.Data.Entities.Images.Enums;
using Unite.Data.Entities.Specimens.Enums;

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
                await CreateArchiveEntry(archive, TsvFileNames.Donor, _donorsTsvService.GetDataForGenes(ids));
            
            // if (criteria.Clinical == true)
            //      await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForGenes(ids));

            if (criteria.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatment, _donorsTsvService.GetTreatmentsDataForGenes(ids));

            if (criteria.Mrs == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mr, _imagesTsvService.GetDataForGenes(ids, ImageType.MR));

            if (criteria.Specimens == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.Material, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Material));
                await CreateArchiveEntry(archive, TsvFileNames.Line, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.Organoid, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.Xenograft, _specimensTsvService.GetDataForGenes(ids, SpecimenType.Xenograft));
            }

            if (criteria.Interventions == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LineIntervention, _specimensTsvService.GetInterventionsDataForGenes(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidIntervention, _specimensTsvService.GetInterventionsDataForGenes(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftIntervention, _specimensTsvService.GetInterventionsDataForGenes(ids, SpecimenType.Xenograft));
            }

            if (criteria.Drugs == true)
            {
                await CreateArchiveEntry(archive, TsvFileNames.LineDrug, _specimensTsvService.GetDrugsScreeningsDataForGenes(ids, SpecimenType.Line));
                await CreateArchiveEntry(archive, TsvFileNames.OrganoidDrug, _specimensTsvService.GetDrugsScreeningsDataForGenes(ids, SpecimenType.Organoid));
                await CreateArchiveEntry(archive, TsvFileNames.XenograftDrug, _specimensTsvService.GetDrugsScreeningsDataForGenes(ids, SpecimenType.Xenograft));
            }

            if (criteria.Sms == true)
            {
                if (criteria.SmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Sm, _variantsTsvService.GetFullDataForGenes(ids, VariantType.SM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Sm, _variantsTsvService.GetDataForGenes(ids, VariantType.SM, criteria.SmsTranscriptsSlim ?? false));
            }

            if (criteria.Cnvs == true)
            {
                if (criteria.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnv, _variantsTsvService.GetFullDataForGenes(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnv, _variantsTsvService.GetDataForGenes(ids, VariantType.CNV, criteria.CnvsTranscriptsSlim ?? false));
            }

            if (criteria.Svs == true)
            {
                if (criteria.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Sv, _variantsTsvService.GetFullDataForGenes(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Sv, _variantsTsvService.GetDataForGenes(ids, VariantType.SV, criteria.SvsTranscriptsSlim ?? false));
            }

            if (criteria.GeneExp == true)
                await CreateArchiveEntry(archive, TsvFileNames.GeneExp, _transcriptomicsTsvService.GetData(ids));
                
            archive.Dispose();
            archiveStream.Close();
            archiveBytes = archiveStream.ToArray();
        }

        return archiveBytes;
    }
}
