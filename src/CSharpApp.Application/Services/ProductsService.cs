using System.Text;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IRestApiService _restApiService;

    public ProductsService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<ProductsService> logger, IRestApiService restApiService)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;

        _httpClient.BaseAddress = new Uri(_restApiSettings.BaseUrl!);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        _restApiService = restApiService;
    }

    public async Task<IReadOnlyCollection<Product>> GetProductsAsync()
    {
        try
        {
            _logger.LogInformation("Fetching products from {Url}", _restApiSettings.Endpoints.GetAllProducts);

            var request = await _restApiService.CreateRequestAsync(HttpMethod.Get, _restApiSettings.Endpoints.GetAllProducts);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch products. Status Code: {StatusCode}", response.StatusCode);
                return new List<Product>().AsReadOnly();
            }

            var content = await response.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<List<Product>>(content, _jsonOptions);

            return products?.AsReadOnly() ?? new List<Product>().AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products.");
            return new List<Product>().AsReadOnly();
        }
    }

    public async Task<Product> GetOneProductAsync(int prodId)
    {
        try
        {
            _logger.LogInformation("Fetching product from {Url}", _restApiSettings.Endpoints.GetProductById.Replace("{id}", prodId.ToString()));

            var request = await _restApiService.CreateRequestAsync(HttpMethod.Get, _restApiSettings.Endpoints.GetProductById.Replace("{id}", prodId.ToString()));
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch product. Status Code: {StatusCode}", response.StatusCode);
                return new Product();
            }

            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, _jsonOptions);

            return product ?? new Product();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching product.");
            return new Product();
        }
    }

    public async Task<Product> CreateProductAsync(CreateProductDto newProduct)
    {
        try
        {
            _logger.LogInformation("Create product with jsonString: ", JsonSerializer.Serialize(newProduct));

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(newProduct),
                Encoding.UTF8,
                "application/json"
            );

            var request = await _restApiService.CreateRequestAsync(HttpMethod.Post, _restApiSettings.Endpoints.CreateProduct, jsonContent);
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to create product. Status Code: {StatusCode}", response.StatusCode);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(content, _jsonOptions);

            return product ?? new Product();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating products.");
            return new Product();
        }
    }
}
