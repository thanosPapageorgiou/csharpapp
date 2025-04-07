namespace CSharpApp.Core.Interfaces;

public interface IProductsService
{
    Task<Result<IReadOnlyCollection<Product>>> GetProducts();
    Task<Result<Product>> GetProduct(int id);
    Task<Result<Product>> CreateProduct(CreateProductRequest request);
}