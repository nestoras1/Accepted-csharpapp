namespace CSharpApp.Core.Settings;

public sealed class RestApiSettings
{
    public string? BaseUrl { get; set; }
    public EndpointsSettings Endpoints { get; set; } = new();
    public string? Auth { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
}