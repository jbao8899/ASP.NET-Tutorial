using System.Text.Json;

namespace API_Consumer.Data
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "ShirtsApi";
        private readonly IHttpClientFactory _httpClientFactory;

        public WebApiExecutor(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);

            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);

            await HandlePotentialError(response);

            //return await httpClient.GetFromJsonAsync<T>(relativeUrl);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);
            //response.EnsureSuccessStatusCode();

            await HandlePotentialError(response); 

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj) 
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            //response.EnsureSuccessStatusCode();
            await HandlePotentialError(response);
        }

        public async Task InvokeDelete<T>(string relativeUrl)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            var response = await httpClient.DeleteAsync(relativeUrl);
            //response.EnsureSuccessStatusCode();
            await HandlePotentialError(response);
        }

        private async Task HandlePotentialError(HttpResponseMessage httpResponseMessage)
        {
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                string errorJson = await httpResponseMessage.Content.ReadAsStringAsync();
                throw new WebApiException(errorJson);
            }
        }
    }
}
