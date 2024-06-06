using System.IO.Compression;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Tsv.Constants;
using Unite.Composer.Download.Tsv.Mapping;
using Unite.Data.Entities.Genome.Analysis.Dna.Enums;
using Unite.Data.Entities.Images.Enums;
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
                await CreateArchiveEntry(archive, TsvFileNames.Donors, _donorsTsvService.GetDataForSpecimens(ids));
            
            if (criteria.Clinical == true)
                await CreateArchiveEntry(archive, TsvFileNames.Clinical, _donorsTsvService.GetClinicalDataForSpecimens(ids));

            if (criteria.Treatments == true)
                await CreateArchiveEntry(archive, TsvFileNames.Treatments, _donorsTsvService.GetTreatmentsDataForSpecimens(ids));

            if (criteria.Mris == true)
                await CreateArchiveEntry(archive, TsvFileNames.Mris, _imagesTsvService.GetDataForSpecimens(ids, ImageType.MRI));

            if (criteria.Specimens == true)
            {
                if (type == SpecimenType.Material)
                    await CreateArchiveEntry(archive, TsvFileNames.Materials, _specimensTsvService.GetData(ids, SpecimenType.Material));

                if (type == SpecimenType.Line)
                    await CreateArchiveEntry(archive, TsvFileNames.Lines, _specimensTsvService.GetData(ids, SpecimenType.Line));

                if (type == SpecimenType.Organoid)
                    await CreateArchiveEntry(archive, TsvFileNames.Organoids, _specimensTsvService.GetData(ids, SpecimenType.Organoid));

                if (type == SpecimenType.Xenograft)
                    await CreateArchiveEntry(archive, TsvFileNames.Xenografts, _specimensTsvService.GetData(ids, SpecimenType.Xenograft));
            }

            if (criteria.Interventions == true)
            {
                if (type == SpecimenType.Line)
                    await CreateArchiveEntry(archive, TsvFileNames.LinesInterventions, _specimensTsvService.GetInterventionsData(ids, SpecimenType.Line));

                if (type == SpecimenType.Organoid)
                    await CreateArchiveEntry(archive, TsvFileNames.OrganoidsInterventions, _specimensTsvService.GetInterventionsData(ids, SpecimenType.Organoid));

                if (type == SpecimenType.Xenograft)
                    await CreateArchiveEntry(archive, TsvFileNames.XenograftsInterventions, _specimensTsvService.GetInterventionsData(ids, SpecimenType.Xenograft));
            }

            if (criteria.Drugs == true)
            {
                if (type == SpecimenType.Line)
                    await CreateArchiveEntry(archive, TsvFileNames.LinesDrugs, _specimensTsvService.GetDrugsScreeningsData(ids, SpecimenType.Line));

                if (type == SpecimenType.Organoid)
                    await CreateArchiveEntry(archive, TsvFileNames.OrganoidsDrugs, _specimensTsvService.GetDrugsScreeningsData(ids, SpecimenType.Organoid));

                if (type == SpecimenType.Xenograft)
                    await CreateArchiveEntry(archive, TsvFileNames.XenograftsDrugs, _specimensTsvService.GetDrugsScreeningsData(ids, SpecimenType.Xenograft));
            }

            if (criteria.Ssms == true)
            {
                if (criteria.SsmsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetFullDataForSpecimens(ids, VariantType.SSM));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Ssms, _variantsTsvService.GetDataForSpecimens(ids, VariantType.SSM, criteria.SsmsTranscriptsSlim ?? false));
            }

            if (criteria.Cnvs == true)
            {
                if (criteria.CnvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetFullDataForSpecimens(ids, VariantType.CNV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Cnvs, _variantsTsvService.GetDataForSpecimens(ids, VariantType.CNV, criteria.CnvsTranscriptsSlim ?? false));
            }

            if (criteria.Svs == true)
            {
                if (criteria.SvsTranscriptsFull == true)
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetFullDataForSpecimens(ids, VariantType.SV));
                else
                    await CreateArchiveEntry(archive, TsvFileNames.Svs, _variantsTsvService.GetDataForSpecimens(ids, VariantType.SV, criteria.SvsTranscriptsSlim ?? false));
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
