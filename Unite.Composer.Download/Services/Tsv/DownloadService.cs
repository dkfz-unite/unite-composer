using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Services.Tsv.Constants;
using Unite.Composer.Download.Services.Tsv.Mapping;
using Unite.Data.Context;
using Unite.Essentials.Extensions;
using Unite.Essentials.Tsv;

using SM = Unite.Data.Entities.Omics.Analysis.Dna.Sm;
using CNV = Unite.Data.Entities.Omics.Analysis.Dna.Cnv;
using SV = Unite.Data.Entities.Omics.Analysis.Dna.Sv;


namespace Unite.Composer.Download.Services.Tsv;

public abstract class DownloadService : Services.DownloadService
{
    public DownloadService(IDbContextFactory<DomainDbContext> dbContextFactory) : base(dbContextFactory)
    {
    }

    public override async Task Download(IEnumerable<int> ids, DataTypesCriteria criteria, ZipArchive archive)
    {
        if (criteria.Donor == true)
        {
            var donors = await GetDonors(ids);
            WriteData(archive, FileNames.Donor, donors, DonorMapper.GetDonorMap());
        }

        if (criteria.Treatment == true)
        {
            var treatments = await GetTreatments(ids);
            WriteData(archive, FileNames.Treatment, treatments, DonorMapper.GetTreatmentMap());
        }

        if (criteria.Image == true)
        {
            var images = await GetImages(ids);
            var groups = images.GroupBy(entity => entity.TypeId);

            foreach (var group in groups)
            {
                var type = group.Key.ToDefinitionString().ToLower();
                WriteData(archive, string.Format(FileNames.Image, type), group.ToArray(), ImageMapper.GetImageMap(group.Key));
            }
        }

        if (criteria.Specimen == true)
        {
            var specimens = await GetSpecimens(ids);
            var groups = specimens.GroupBy(entity => entity.TypeId);

            foreach (var group in groups)
            {
                var type = group.Key.ToDefinitionString().ToLower();
                WriteData(archive, string.Format(FileNames.Specimen, type), group.ToArray(), SpecimenMapper.GetSpecimenMap(group.Key));
            }
        }

        if (criteria.Intervention == true)
        {
            var interventions = await GetInterventions(ids);
            var groups = interventions.GroupBy(entity => entity.Specimen.TypeId);

            foreach (var group in groups)
            {
                var type = group.Key.ToDefinitionString().ToLower();
                WriteData(archive, string.Format(FileNames.Intervention, type), group.ToArray(), SpecimenMapper.GetInterventionMap());
            }
        }

        if (criteria.Drug == true)
        {
            foreach (var chunk in ids.Chunk(100))
            {
                var drugs = await GetDrugScreenings(chunk);
                var groups = drugs.GroupBy(entry => entry.SampleId);
                var samples = (await GetSpecimenSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(FileNames.DrugScreening, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), SpecimenAnalysisMapper.GetDrugScreeningMap(), comments: SpecimenAnalysisMapper.MapSample(sample));
                }
            }
        }

        if (criteria.Sm == true)
        {
            foreach (var chunk in ids.Chunk(50))
            {
                var variants = await GetSmVariants(chunk, criteria.SmTranscript ?? false);
                var groups = variants.GroupBy(entry => entry.SampleId);
                var samples = (await GetDnaSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(FileNames.Sm, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), DnaAnalysisMapper.GetVariantMap<SM.VariantEntry, SM.Variant>(criteria.SmTranscript ?? false), comments: OmicsAnalysisMapper.MapSample(sample));
                }
            }
        }

        if (criteria.Cnv == true)
        {
            foreach (var chunk in ids.Chunk(25))
            {
                var variants = await GetCnvVariants(chunk, criteria.CnvTranscript ?? false);
                var groups = variants.GroupBy(entry => entry.SampleId);
                var samples = (await GetDnaSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(FileNames.Cnv, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), DnaAnalysisMapper.GetVariantMap<CNV.VariantEntry, CNV.Variant>(criteria.CnvTranscript ?? false), comments: OmicsAnalysisMapper.MapSample(sample));
                }
            }
        }

        if (criteria.Sv == true)
        {
            foreach (var chunk in ids.Chunk(25))
            {
                var variants = await GetSvVariants(chunk, criteria.SvTranscript ?? false);
                var groups = variants.GroupBy(entry => entry.SampleId);
                var samples = (await GetDnaSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(FileNames.Sv, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), DnaAnalysisMapper.GetVariantMap<SV.VariantEntry, SV.Variant>(criteria.SvTranscript ?? false), comments: OmicsAnalysisMapper.MapSample(sample));
                }
            }
        }

        if (criteria.GeneExp == true)
        {
            foreach (var chunk in ids.Chunk(25))
            {
                var expressions = await GetGeneExpressions(chunk);
                var groups = expressions.GroupBy(entry => entry.SampleId);
                var samples = (await GetRnaSamples(groups.Select(group => group.Key))).ToDictionary(sample => sample.Id);

                foreach (var group in groups)
                {
                    var sample = samples[group.Key];
                    var entryName = string.Format(FileNames.GeneExp, sample.Specimen.Donor.ReferenceId, sample.Specimen.ReferenceId, sample.Specimen.TypeId.ToDefinitionString());
                    WriteData(archive, entryName, group.ToArray(), RnaAnalysisMapper.GetExpressionMap(), comments: OmicsAnalysisMapper.MapSample(sample));
                }
            }
        }
    }


    protected abstract Task<Data.Entities.Donors.Donor[]> GetDonors(IEnumerable<int> ids);
    protected abstract Task<Data.Entities.Donors.Clinical.Treatment[]> GetTreatments(IEnumerable<int> ids);

    protected abstract Task<Data.Entities.Images.Image[]> GetImages(IEnumerable<int> ids);
    protected abstract Task<Data.Entities.Images.Analysis.Sample[]> GetImageSamples(IEnumerable<int> ids);

    protected abstract Task<Data.Entities.Specimens.Specimen[]> GetSpecimens(IEnumerable<int> ids);
    protected abstract Task<Data.Entities.Specimens.Intervention[]> GetInterventions(IEnumerable<int> ids);
    protected abstract Task<Data.Entities.Specimens.Analysis.Sample[]> GetSpecimenSamples(IEnumerable<int> ids);
    protected abstract Task<Data.Entities.Specimens.Analysis.Drugs.DrugScreening[]> GetDrugScreenings(IEnumerable<int> ids);

    protected abstract Task<Data.Entities.Omics.Analysis.Sample[]> GetDnaSamples(IEnumerable<int> ids);
    protected abstract Task<SM.VariantEntry[]> GetSmVariants(IEnumerable<int> ids, bool transcripts);
    protected abstract Task<CNV.VariantEntry[]> GetCnvVariants(IEnumerable<int> ids, bool transcripts);
    protected abstract Task<SV.VariantEntry[]> GetSvVariants(IEnumerable<int> ids, bool transcripts);


    protected abstract Task<Data.Entities.Omics.Analysis.Sample[]> GetRnaSamples(IEnumerable<int> ids);
    protected abstract Task<Data.Entities.Omics.Analysis.Rna.GeneExpression[]> GetGeneExpressions(IEnumerable<int> ids);
     


    private static void WriteData<T>(ZipArchive archive, string name, T[] data, ClassMap<T> map, string[] comments = null) where T : class
    {
        if (data == null || data.Length == 0)
            return;

        using var writer = CreateEntryWriter(archive, name);
        TsvWriter.Write(writer, data, map: map, comments: comments);
    }
}
