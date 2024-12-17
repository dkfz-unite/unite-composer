using Unite.Cache.Configuration.Options;
using Unite.Composer.Data.Datasets.Models;

namespace Unite.Composer.Data.Datasets;

public class DatasetsService
{
    private readonly Repositories.DatasetsRepository _datasetsRepository;

    public DatasetsService(IMongoOptions options)
	{
		_datasetsRepository = new Repositories.DatasetsRepository(options);
	}

	public async Task<IEnumerable<DatasetModel>> Load(DatasetModel data)
	{
		var datasets = (await _datasetsRepository.
		WhereAsync(item =>item.Document.UserID == data.UserID && item.Document.Domain == data.Domain))
		.Select(item => item.Document with {Id = item.Id});
		
		return datasets;
	}
}