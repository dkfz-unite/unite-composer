using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients
{
    internal class XmlHttpClient : IDisposable
    {
        private readonly HttpClient _httpClient;


        public XmlHttpClient()
        {
            var handler = new HttpClientHandler { UseProxy = false };
            _httpClient = new HttpClient(handler);
        }

        public XmlHttpClient(string baseUrl) : this()
        {
            _httpClient.BaseAddress = new Uri(baseUrl);
        }


        public async Task<T> GetAsync<T>(string url, params (string name, string value)[] headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            AddRequestHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var dataXml = await response.Content.ReadAsStringAsync();

                using var stringReader = new StringReader(dataXml);

                var serializer = new XmlSerializer(typeof(T));
                var data = (T)serializer.Deserialize(stringReader);

                return data;
            }
            else
            {
                var message = await response.Content?.ReadAsStringAsync();
                throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase} - {message}");
            }
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
}
