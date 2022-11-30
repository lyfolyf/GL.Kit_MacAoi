using Newtonsoft.Json;

namespace System.Net.Http
{
    public static class HttpResponseExtension
    {
        public static T Deserialize<T>(this HttpResponse response)
        {
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(response.Content);
            }

            throw new Exception(response.ReasonPhrase + "\r\n" + response.Content);
        }

        public static void Deserialize(this HttpResponse response)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase + "\r\n" + response.Content);
            }
        }
    }
}
