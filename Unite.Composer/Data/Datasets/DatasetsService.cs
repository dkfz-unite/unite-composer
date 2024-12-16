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

    public string AddDataset(DatasetsModel data)
	{
	 	return _datasetsRepository.Add(data);
	}

	public List<DatasetsModel> LoadDatasets(DatasetsModel data)
	{
	 	var datasets = _datasetsRepository.Where(item =>item.Document.UserID == data.UserID && item.Document.Domain == data.Domain);
	    
		List<DatasetsModel> datasetsModel = new List<DatasetsModel>();
		foreach(var item in datasets)
		{
			DatasetsModel dataset = new DatasetsModel {
				UserID = item.Document.UserID,
				Domain = item.Document.Domain,
				Name = item.Document.Name,
				Description = item.Document.Description,
				Date = item.Document.Date,
				Criteria = item.Document.Criteria,
				Id = item.Id
			};
			datasetsModel.Add(dataset);
		}
		
		return datasetsModel;
	}

	public void DeleteDataset(string id)
	{
	 	_datasetsRepository.Delete(id);
	}
}