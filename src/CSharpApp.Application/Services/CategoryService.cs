using System.Text;

namespace CSharpApp.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<CategoryService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IRestApiService _restApiService;

        public CategoryService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CategoryService> logger, IRestApiService restApiService)
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

        public async Task<IReadOnlyCollection<Category>> GetCategoriesAsync()
        {
            try
            {
                _logger.LogInformation("Fetching categories from {Url}", _restApiSettings.Endpoints.GetAllCategories);

                var request = await _restApiService.CreateRequestAsync(HttpMethod.Get, _restApiSettings.Endpoints.GetAllCategories);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch categories. Status Code: {StatusCode}", response.StatusCode);
                    return new List<Category>().AsReadOnly();
                }

                var content = await response.Content.ReadAsStringAsync();
                var categories = JsonSerializer.Deserialize<List<Category>>(content, _jsonOptions);

                return categories?.AsReadOnly() ?? new List<Category>().AsReadOnly();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                return new List<Category>().AsReadOnly();
            }
        }

        public async Task<Category> GetOneCategoryAsync(int categoryId)
        {
            try
            {
                _logger.LogInformation("Fetching category from {Url}", _restApiSettings.Endpoints.GetCategoryById.Replace("{id}", categoryId.ToString()));

                var request = await _restApiService.CreateRequestAsync(HttpMethod.Get, _restApiSettings.Endpoints.GetCategoryById.Replace("{id}", categoryId.ToString()));
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to fetch category. Status Code: {StatusCode}", response.StatusCode);
                    return new Category();
                }

                var content = await response.Content.ReadAsStringAsync();
                var category = JsonSerializer.Deserialize<Category>(content, _jsonOptions);

                return category ?? new Category();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                return new Category();
            }
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto newCategory)
        {
            try
            {
                _logger.LogInformation("Create category with jsonString: ", JsonSerializer.Serialize(newCategory));

                var jsonContent = new StringContent(
                    JsonSerializer.Serialize(newCategory),
                    Encoding.UTF8,
                    "application/json"
                );

                var request = await _restApiService.CreateRequestAsync(HttpMethod.Post, _restApiSettings.Endpoints.CreateCategory, jsonContent);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Failed to create category. Status Code: {StatusCode}", response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var category = JsonSerializer.Deserialize<Category>(content, _jsonOptions);

                return category ?? new Category();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating category.");
                return new Category();
            }
        }
    }
}
