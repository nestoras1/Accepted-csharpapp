namespace CSharpApp.Core.Interfaces
{
    public interface IRestApiService
    {
        Task<HttpRequestMessage> CreateRequestAsync(HttpMethod method, string endpoint, HttpContent? content = null);
    }
}
