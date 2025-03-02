using CSharpApp.Application.Services;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

public class ApiServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly Mock<IOptions<RestApiSettings>> _restApiSettingsMock;
    private readonly Mock<ILogger<CategoryService>> _loggerMock;
    private readonly Mock<ILogger<ProductsService>> _mockProductLogger;
    private readonly Mock<IRestApiService> _restApiServiceMock;
    private readonly CategoryService _categoryService;
    private readonly ProductsService _productsService;

    public ApiServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("https://api.escuelajs.co/api/v1/")
        };

        _restApiSettingsMock = new Mock<IOptions<RestApiSettings>>();
        _loggerMock = new Mock<ILogger<CategoryService>>();
        _mockProductLogger = new Mock<ILogger<ProductsService>>();
        _restApiServiceMock = new Mock<IRestApiService>();

        _restApiSettingsMock.Setup(x => x.Value).Returns(new RestApiSettings
        {
            BaseUrl = "https://api.escuelajs.co/api/v1/",
            Endpoints = new EndpointsSettings
            {
                GetAllProducts = "products/",
                GetProductById = "products/{id}",
                CreateProduct = "products/",
                GetAllCategories = "categories",
                GetCategoryById = "categories/{id}",
                CreateCategory = "categories/"
            }
        });

        _categoryService = new CategoryService(
            _httpClient,
            _restApiSettingsMock.Object,
            _loggerMock.Object,
            _restApiServiceMock.Object
        );

        _productsService = new ProductsService(
            _httpClient,
            _restApiSettingsMock.Object,
            _mockProductLogger.Object,
            _restApiServiceMock.Object
        );
    }

    [Fact]
    public async Task GetCategoriesAsync_ShouldReturnCategories_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedCategories = new List<Category>
        {
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" }
        };

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(expectedCategories))
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "categories", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _categoryService.GetCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategories.Count, result.Count);
        Assert.Equal(expectedCategories[0].Id, result.FirstOrDefault().Id);
    }

    [Fact]
    public async Task GetCategoriesAsync_ShouldReturnEmptyList_WhenApiCallFails()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "categories", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _categoryService.GetCategoriesAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOneCategoryAsync_ShouldReturnCategory_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedCategory = new Category { Id = 1, Name = "Electronics" };

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(expectedCategory))
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "categories/1", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _categoryService.GetOneCategoryAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategory.Id, result.Id);
        Assert.Equal(expectedCategory.Name, result.Name);
    }

    [Fact]
    public async Task GetOneCategoryAsync_ShouldReturnEmptyCategory_WhenApiCallFails()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "categories/99999", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _categoryService.GetOneCategoryAsync(99999);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Id);
    }

    // Test Product Services
    [Fact]
    public async Task GetProductsAsync_ShouldReturnProducts_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Title = "Laptop" },
            new Product { Id = 2, Title = "Smartphone" }
        };

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(expectedProducts))
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "products/", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _productsService.GetProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProducts.Count, result.Count);
        Assert.Equal(expectedProducts[0].Id, result.FirstOrDefault().Id);
    }

    [Fact]
    public async Task GetProductsAsync_ShouldReturnEmptyList_WhenApiCallFails()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.InternalServerError
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "products/", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _productsService.GetProductsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetOneProductAsync_ShouldReturnProduct_WhenApiCallIsSuccessful()
    {
        // Arrange
        var expectedProduct = new Product { Id = 1, Title = "Laptop" };

        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(expectedProduct))
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "products/1", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _productsService.GetOneProductAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Title, result.Title);
    }

    [Fact]
    public async Task GetOneProductAsync_ShouldReturnEmptyProduct_WhenApiCallFails()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        _restApiServiceMock
            .Setup(x => x.CreateRequestAsync(HttpMethod.Get, "products/99999", It.IsAny<HttpContent?>()))
            .ReturnsAsync(new HttpRequestMessage());

        // Act
        var result = await _productsService.GetOneProductAsync(99999);

        // Assert
        Assert.NotNull(result);
        Assert.Null(result.Id);
        Assert.Null(result.Title);
    }
}
