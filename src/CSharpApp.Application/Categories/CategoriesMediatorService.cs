using CSharpApp.Application.Categories.Commands;
using CSharpApp.Application.Categories.Queries;
using MediatR;

namespace CSharpApp.Application.Categories;

public class CategoriesMediatorService : ICategoriesMediatorService
{
    #region Properties
    private readonly IMediator _mediator;
    #endregion

    #region Constructor
    public CategoriesMediatorService(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Public Methods
    public async Task<Result<IReadOnlyCollection<Category>>> GetCategories()
    {
        var categories = await _mediator.Send(new GetCategoryListQuery());

        return categories;
    }
    public async Task<Result<Category>> GetCategory(int categoryId)
    {
        var category = await _mediator.Send(new GetCategoryQuery(categoryId));
        return category;
    }
    public async Task<Result<Category>> CreateCategory(CreateCategory request)
    {
        var category = await _mediator.Send(new CreateCategoryCommand(request));
        return category;
    }
    #endregion
}