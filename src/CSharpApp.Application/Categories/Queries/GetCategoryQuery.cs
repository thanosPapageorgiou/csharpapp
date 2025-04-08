using MediatR;

namespace CSharpApp.Application.Categories.Queries
{
    public record GetCategoryQuery(int CategoryId): IRequest<Result<Category>>;
}
