using System.IO.Compression;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Unite.Composer.Download.Models;
using Unite.Composer.Download.Repositories;
using Unite.Composer.Download.Tsv.Models;
using Unite.Data.Context;

namespace Unite.Composer.Download.Services;

public abstract class DownloadService
{
    protected readonly IDbContextFactory<DomainDbContext> _dbContextFactory;
    protected readonly DonorsDataRepository _donorsDataRepository;
    protected readonly ImagesDataRepository _imagesDataRepository;
    protected readonly SpecimensDataRepository _specimensDataRepository;
    protected readonly SamplesDataRepository _samplesDataRepository;
    protected readonly VariantsDataRepository _variantsDataRepository;
    protected readonly GeneExpressionsDataRepository _geneExpressionsDataRepository;


    public DownloadService(IDbContextFactory<DomainDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _donorsDataRepository = new DonorsDataRepository(_dbContextFactory);
        _imagesDataRepository = new ImagesDataRepository(_dbContextFactory);
        _specimensDataRepository = new SpecimensDataRepository(_dbContextFactory);
        _samplesDataRepository = new SamplesDataRepository(_dbContextFactory);
        _variantsDataRepository = new VariantsDataRepository(_dbContextFactory);
        _geneExpressionsDataRepository = new GeneExpressionsDataRepository(_dbContextFactory);
    }


    // public abstract Task Download(IEnumerable<int> ids, DownloadCriteria criteria, ZipArchive archive);
    public abstract Task Download(IEnumerable<int> ids, DataTypesCriteria criteria, ZipArchive archive);



    protected static StreamWriter CreateEntryWriter(ZipArchive archive, string name)
    {
        var entry = archive.CreateEntry(name);
        var entryStream = entry.Open();
        return new StreamWriter(entryStream, Encoding.UTF8);
    }
}
