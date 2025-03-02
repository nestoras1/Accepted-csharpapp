using CSharpApp.Core.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CSharpApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A list of categories.</returns>
        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Category>>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Fetching Categories...");

            var categories = await _categoryService.GetCategoriesAsync();

            if (categories.Count == 0)
            {
                _logger.LogWarning("No category found.");
                return NoContent();
            }

            return Ok(categories);
        }

        /// <summary>
        /// Retrieves a category.
        /// </summary>
        /// <returns>One category.</returns>
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Category>> GetOneCategoryAsync(int categoryId)
        {
            _logger.LogInformation("Fetching Category...");

            var category = await _categoryService.GetOneCategoryAsync(categoryId);

            if (category == null)
            {
                _logger.LogWarning("No category found.");
                return NoContent();
            }

            return Ok(category);
        }

        /// <summary>
        /// Create a category.
        /// </summary>
        /// <returns>Created category.</returns>
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategoryAsync([FromBody] CreateCategoryDto category)
        {
            _logger.LogInformation("Create Category...");

            var response = await _categoryService.CreateCategoryAsync(category);

            if (response == null)
            {
                _logger.LogWarning("Failed create category.");
                return BadRequest();
            }

            return Ok(response);
        }
    }
}
