namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<IReadOnlyCollection<Product>> GetProductsAsync();
    Task<Product> GetOneProductAsync(int prodId);
    Task<Product> CreateProductAsync(CreateProductDto newProduct);
}