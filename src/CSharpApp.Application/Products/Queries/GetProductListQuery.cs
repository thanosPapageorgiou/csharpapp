using MediatR;

namespace CSharpApp.Application.Products.Queries
{
    public record GetProductListQuery(): IRequest<Result<IReadOnlyCollection<Product>>>;
}
