using Microsoft.EntityFrameworkCore;
using Unite.Data.Context;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;

namespace Unite.Composer.Download.Services.Tsv;

public class DonorsDownloadService : DownloadService
{
    public DonorsDownloadService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory) { }


    protected override Task<Data.Entities.Donors.Donor[]> GetDonors(IEnumerable<int> ids)
    {
        return _donorDataRepository.GetDonors(ids);
    }

    protected override Task<Data.Entities.Donors.Clinical.Treatment[]> GetTreatments(IEnumerable<int> ids)
    {
        return _donorDataRepository.GetTreatments(ids);
    }


    protected override Task<Data.Entities.Images.Image[]> GetImages(IEnumerable<int> ids)
    {
        return _imageDataRepository.GetImagesForDonors(ids);
    }

    protected override Task<Data.Entities.Images.Analysis.Sample[]> GetImageSamples(IEnumerable<int> ids)
    {
        return _imageAnalysisDataRepository.GetSamples(ids);
    }


    protected override Task<Data.Entities.Specimens.Specimen[]> GetSpecimens(IEnumerable<int> ids)
    {
        return _specimenDataRepository.GetSpecimensForDonors(ids);
    }

    protected override Task<Data.Entities.Specimens.Intervention[]> GetInterventions(IEnumerable<int> ids)
    {
        return _specimenDataRepository.GetInterventionsForDonors(ids);
    }

    protected override Task<Data.Entities.Specimens.Analysis.Sample[]> GetSpecimenSamples(IEnumerable<int> ids)
    {
        return _specimenAnalysisDataRepository.GetSamples(ids);
    }

    protected override Task<Data.Entities.Specimens.Analysis.Drugs.DrugScreening[]> GetDrugScreenings(IEnumerable<int> ids)
    {
        return _specimenAnalysisDataRepository.GetDrugsForDonors(ids);
    }


    protected override Task<Data.Entities.Omics.Analysis.Sample[]> GetDnaSamples(IEnumerable<int> ids)
    {
        return _dnaAnalysisDataRepository.GetSamples(ids);
    }

    protected override Task<SM.VariantEntry[]> GetSmVariants(IEnumerable<int> ids, bool transcripts)
    {
        return _dnaAnalysisDataRepository.GetVariantsForDonors<SM.VariantEntry, SM.Variant>(ids, transcripts);
    }

    protected override Task<CNV.VariantEntry[]> GetCnvVariants(IEnumerable<int> ids, bool transcripts)
    {
        return _dnaAnalysisDataRepository.GetVariantsForDonors<CNV.VariantEntry, CNV.Variant>(ids, transcripts);
    }

    protected override Task<SV.VariantEntry[]> GetSvVariants(IEnumerable<int> ids, bool transcripts)
    {
        return _dnaAnalysisDataRepository.GetVariantsForDonors<SV.VariantEntry, SV.Variant>(ids, transcripts);
    }


    protected override Task<Data.Entities.Omics.Analysis.Sample[]> GetRnaSamples(IEnumerable<int> ids)
    {
        return _rnaAnalysisDataRepository.GetSamples(ids);
    }

    protected override Task<Data.Entities.Omics.Analysis.Rna.GeneExpression[]> GetGeneExpressions(IEnumerable<int> ids)
    {
        return _rnaAnalysisDataRepository.GetExpressionsForDonors(ids);
    }
}
