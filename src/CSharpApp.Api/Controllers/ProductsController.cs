using CSharpApp.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsService productsService, ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Product>>> GetAllProductsAsync()
        {
            _logger.LogInformation("Fetching products...");

            var products = await _productsService.GetProductsAsync();

            if (products.Count == 0)
            {
                _logger.LogWarning("No products found.");
                return NoContent();
            }

            return Ok(products);
        }

        /// <summary>
        /// Retrieves a product.
        /// </summary>
        /// <returns>One product.</returns>
        [HttpGet("{prodId}")]
        public async Task<ActionResult<Product>> GetOneProductAsync(int prodId)
        {
            _logger.LogInformation("Fetching product...");

            var product = await _productsService.GetOneProductAsync(prodId);

            if (product == null)
            {
                _logger.LogWarning("No product found.");
                return NoContent();
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <returns>Created product.</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProductAsync([FromBody] CreateProductDto product)
        {
            _logger.LogInformation("Create product...");

            var response = await _productsService.CreateProductAsync(product);

            if (response == null)
            {
                _logger.LogWarning("Failed create product.");
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
