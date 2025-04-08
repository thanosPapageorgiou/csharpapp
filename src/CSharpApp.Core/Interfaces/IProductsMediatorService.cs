namespace CSharpApp.Core.Interfaces;

public interface IProductsMediatorService
{
    Task<Result<IReadOnlyCollection<Product>>> GetProducts();
    Task<Result<Product>> GetProduct(int productId);
    Task<Result<Product>> CreateProduct(CreateProduct request);
}