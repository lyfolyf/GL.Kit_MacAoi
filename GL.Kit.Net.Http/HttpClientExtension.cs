using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace System.Net.Http
{
    public static class HttpClientExtension
    {
        const string Authorization = "Authorization";

        public async static Task<HttpResponse> GetAsync2(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);

            return await ReadResponse(response);
        }

        public async static Task<HttpResponse> GetAsync2(this HttpClient client, string hostRoute,
            IDictionary<string, string> headers = null, IDictionary<string, object> queries = null)
        {
            Url url = Url.Parse(hostRoute);
            if (queries != null)
                url.Queries = new QueryParamCollection(queries);

            return await GetAsync2(client, url, headers);
        }

        public async static Task<HttpResponse> GetAsync2(this HttpClient client, Url url, IDictionary<string, string> headers = null)
        {
            HttpRequestMessage httpRequest = new HttpRequestMessage(HttpMethod.Get, url.ToString());

            SetHeaders(httpRequest.Headers, headers);

            var response = await client.SendAsync(httpRequest);

            return await ReadResponse(response);
        }

        public async static Task<HttpResponse> PostJsonAsync2(this HttpClient client, string url, object entity)
        {
            return await PostJsonAsync2(client, url, null, entity);
        }

        public async static Task<HttpResponse> PostJsonAsync2(this HttpClient client, string url, IDictionary<string, string> headers, object entity)
        {
            string json = JsonConvert.SerializeObject(entity);

            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            SetHeaders(httpContent.Headers, headers);

            var response = await client.PostAsync(url, httpContent);

            return await ReadResponse(response);
        }

        public async static Task<HttpResponse> DeleteAsync2(this HttpClient client, string url)
        {
            var response = await client.DeleteAsync(url);

            return await ReadResponse(response);
        }

        public async static Task<HttpResponse> PutJsonAsync2(this HttpClient client, string url, object entity)
        {
            return await PutJsonAsync2(client, url, null, entity);
        }

        public async static Task<HttpResponse> PutJsonAsync2(this HttpClient client, string url, IDictionary<string, string> headers, object entity)
        {
            string json = JsonConvert.SerializeObject(entity);

            HttpContent httpContent = new StringContent(json);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            SetHeaders(httpContent.Headers, headers);

            var response = await client.PutAsync(url, httpContent);

            return await ReadResponse(response);
        }

        private static void SetHeaders(HttpHeaders headers, IDictionary<string, string> headerParams)
        {
            if (headerParams != null)
            {
                foreach (var header in headerParams)
                {
                    if (!string.IsNullOrWhiteSpace(header.Value))
                        headers.Add(header.Key, header.Value);
                }
            }
        }

        private async static Task<HttpResponse> ReadResponse(HttpResponseMessage response)
        {
            return new HttpResponse
            {
                IsSuccessStatusCode = response.IsSuccessStatusCode,
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                Content = await response.Content.ReadAsStringAsync(),
                Authorization = response.Headers.Contains(Authorization) ? response.Headers.GetValues(Authorization).FirstOrDefault() : null
            };
        }
    }
}

