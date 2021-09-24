using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

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

        /// <summary>
        /// Sends http GET request to given URL with given request headers.
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Response in text/plain format.</returns>
        public async Task<string> GetAsync(string url, params (string name, string value)[] headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            AddRequestHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                return responseData;
            }
            else
            {
                var message = await response.Content?.ReadAsStringAsync();

                throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase} - {message}");
            }
        }

        /// <summary>
        /// Sends http POST request to given URL with given data and request headers.
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="data">Request data in application/x-www-form-urlencoded format</param>
        /// <param name="headers">Request headers</param>
        /// <returns>Response in text/plain format.</returns>
        public async Task<string> PostAsync(string url, KeyValuePair<string, string>[] data, params (string name, string value)[] headers)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            var content = data == null ? null : new FormUrlEncodedContent(data);

            request.Content = content;

            AddRequestHeaders(request, headers);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();

                return responseData;
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
