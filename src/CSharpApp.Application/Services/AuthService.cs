using System.Text;

namespace CSharpApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        private AuthResponseDto? _cachedTokens;

        public AuthService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings)
        {
            _httpClient = httpClient;
            _restApiSettings = restApiSettings.Value;
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            if (_cachedTokens?.AccessToken is not null)
            {
                return _cachedTokens.AccessToken;
            }

            return await AuthenticateAsync();
        }

        private async Task<string?> AuthenticateAsync()
        {
            var loginRequest = new LoginRequestDto
            {
                Email = _restApiSettings.Username,
                Password = _restApiSettings.Password
            };

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(loginRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(_restApiSettings.BaseUrl + _restApiSettings.Auth, jsonContent);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            _cachedTokens = JsonSerializer.Deserialize<AuthResponseDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return _cachedTokens?.AccessToken;
        }
    }
}
