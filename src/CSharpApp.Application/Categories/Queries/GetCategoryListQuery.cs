using MediatR;

namespace CSharpApp.Application.Categories.Queries
{
    public record GetCategoryListQuery(): IRequest<Result<IReadOnlyCollection<Category>>>;
}
