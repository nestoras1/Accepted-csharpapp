using System.Net.Http.Headers;

namespace CSharpApp.Application.Services
{
    public class RestApiService : IRestApiService
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly IAuthService _authService;

        public RestApiService(HttpClient httpClient, IAuthService authService, IOptions<RestApiSettings> restApiSettings)
        {
            _httpClient = httpClient;
            _authService = authService;
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string endpoint, HttpContent? content = null)
        {
            var token = await _authService.GetAccessTokenAsync();
            var request = new HttpRequestMessage(method, $"{_restApiSettings.BaseUrl}{endpoint}");

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (content != null)
            {
                request.Content = content;
            }

            return request;
        }
    }
}
