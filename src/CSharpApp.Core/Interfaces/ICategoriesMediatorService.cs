namespace CSharpApp.Core.Interfaces;

public interface ICategoriesMediatorService
{
    Task<Result<IReadOnlyCollection<Category>>> GetCategories();
    Task<Result<Category>> GetCategory(int categoryId);
    Task<Result<Category>> CreateCategory(CreateCategory request);
}