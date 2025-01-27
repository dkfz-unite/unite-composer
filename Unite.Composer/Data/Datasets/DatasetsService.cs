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


	public async Task<DatasetModel[]> Load(SearchModel model)
	{
		var datasets = await _datasetsRepository.WhereAsync(item =>item.Document.UserId == model.UserId);
		return datasets.Select(item => item.Document with {Id = item.Id}).ToArray();
	}

	public async Task Delete(SearchModel model)
	{
	 	var datasets = await _datasetsRepository.WhereAsync(item =>item.Document.UserId == model.UserId);
		var tasks = datasets.Select(dataset => _datasetsRepository.DeleteAsync(dataset.Id));
		await Task.WhenAll(tasks);
	}
}
