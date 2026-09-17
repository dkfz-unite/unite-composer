using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Unite.Composer.Clients;

internal class JsonHttpClient : IDisposable
{
    private readonly HttpClient _httpClient;


    public JsonHttpClient(bool useProxy = false)
    {
        var handler = new HttpClientHandler { UseProxy = useProxy };
        _httpClient = new HttpClient(handler);
    }


    public JsonHttpClient(string baseUrl, bool useProxy = false) : this(useProxy)
    {
        _httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<T> GetAsync<T>(string url, params (string name, string value)[] headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        return await SendRequest<T>(request, headers);
    }

    public async Task<T> PostAsync<T, TBody>(string url, TBody body, params (string name, string value)[] headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        var contentJson = JsonSerializer.Serialize(body);
        var content = new StringContent(contentJson, encoding: Encoding.UTF8, "application/json");

        request.Content = content;

        return await SendRequest<T>(request, headers);
    }
    
    public async Task<T> PostAsync<T>(string url, params (string name, string value)[] headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        return await SendRequest<T>(request, headers);
    }
    
    public async Task PostAsync(string url, params (string name, string value)[] headers)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, url);

        await SendRequest(request, headers);
    }

    private async Task<T> SendRequest<T>(HttpRequestMessage request, params (string name, string value)[] headers)
    {
        var response = await GetResponse(request, headers);
        
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumMemberConverter() },
            WriteIndented = true
        };

        var dataJson = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<T>(dataJson, options);

        return data;
    }

    private async Task SendRequest(HttpRequestMessage request, params (string name, string value)[] headers)
    {
        await GetResponse(request, headers);
    }
    
    private async Task<HttpResponseMessage> GetResponse(HttpRequestMessage request, params (string name, string value)[] headers)
    {
        AddRequestHeaders(request, headers);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var message = await response.Content?.ReadAsStringAsync();
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase} - {message}");
        }
        
        return response;
    }
    
    private void AddRequestHeaders(HttpRequestMessage request, params (string name, string value)[] headers)
    {
        if (headers != null)
        {
            foreach (var header in headers)
            {
                request.Headers.Add(header.name, header.value);
            }
        }
    }


    #region IDisposable
    public void Dispose()
    {
        _httpClient.Dispose();
    }
    #endregion
}
