using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace API_Consumer.Data
{
    public class WebApiExecutor : IWebApiExecutor
    {
        private const string apiName = "ShirtsApi";
        private const string authApiName = "AuthorityApi";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        IHttpContextAccessor _httpContextAccessor;

        public WebApiExecutor(IHttpClientFactory httpClientFactory,
                              IConfiguration configuration,
                              IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);

            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);

            await HandlePotentialError(response);

            //return await httpClient.GetFromJsonAsync<T>(relativeUrl);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);
            //response.EnsureSuccessStatusCode();

            await HandlePotentialError(response); 

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T obj) 
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);
            //response.EnsureSuccessStatusCode();
            await HandlePotentialError(response);
        }

        public async Task InvokeDelete<T>(string relativeUrl)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient(apiName);
            await AddJwtToHeader(httpClient);
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

        private async Task AddJwtToHeader(HttpClient httpClient)
        {
            // check if we have a token stored
            JwtToken? token = null;
            string? strToken = _httpContextAccessor.HttpContext?.Session.GetString("access_token"); // or accessToken???
            if (!string.IsNullOrWhiteSpace(strToken))
            {
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);
            }

            // don't have a stored token, or it is expired
            // get a new token
            if (token is null || token.ExpiresAt <= DateTime.UtcNow)
            {
                string clientId = _configuration.GetValue<string>("ClientId");
                string secret = _configuration.GetValue<string>("Secret");

                // 1. Authenticate against the authority
                var authoClient = _httpClientFactory.CreateClient(authApiName);
                var response = await authoClient.PostAsJsonAsync("auth", new AppCredential()
                {
                    ClientId = clientId,
                    Secret = secret
                });
                response.EnsureSuccessStatusCode();

                // 2. Get the JWT token from the authority
                strToken = await response.Content.ReadAsStringAsync();
                token = JsonConvert.DeserializeObject<JwtToken>(strToken);

                _httpContextAccessor.HttpContext?.Session.SetString("access_token", strToken); // or accessToken???
            }

            // 3. Pass the JWT token to the endpoint through HTTP headers
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken);
        }
    }
}
