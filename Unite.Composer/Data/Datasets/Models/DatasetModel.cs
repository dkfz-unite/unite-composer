using System.Text.Json.Serialization;

namespace Unite.Composer.Data.Datasets.Models;

public record DatasetModel
{
    private string _id;
    private string _userId;
    private string _domain;
    private string _name;
    private string _description;
    private string _date;
    private string _criteria;

    [JsonPropertyName("key")]
    public string Id { get => _id?.Trim(); set => _id = value; }

    [JsonPropertyName("userid")]
    public string UserID { get => _userId?.Trim(); set => _userId = value; }

    [JsonPropertyName("domain")]
    public string Domain { get => _domain?.Trim(); set => _domain = value; }

    [JsonPropertyName("name")]
    public string Name { get => _name?.Trim(); set => _name = value; }

    [JsonPropertyName("description")]
    public string Description { get => _description?.Trim(); set => _description = value; }

    [JsonPropertyName("date")]
    public string Date { get => _date; set => _date = value; }

    [JsonPropertyName("criteria")]
    public string Criteria { get => _criteria?.Trim(); set => _criteria = value; }
}
