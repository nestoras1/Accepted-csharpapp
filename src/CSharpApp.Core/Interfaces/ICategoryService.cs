namespace CSharpApp.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<IReadOnlyCollection<Category>> GetCategoriesAsync();
        Task<Category> GetOneCategoryAsync(int categoryId);
        Task<Category> CreateCategoryAsync(CreateCategoryDto newCategory);
    }
}
