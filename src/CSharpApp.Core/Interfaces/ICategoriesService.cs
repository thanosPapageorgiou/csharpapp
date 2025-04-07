namespace CSharpApp.Core.Interfaces;

public interface ICategoriesService
{
    Task<Result<IReadOnlyCollection<Category>>> GetCategories();
    Task<Result<Category>> GetCategory(int id);
    Task<Result<Category>> CreateCategory(CreateCategoryRequest request);
}