using MediatR;

namespace CSharpApp.Application.Products.Queries
{
    public record GetProductQuery(int ProductId): IRequest<Result<Product>>;
}
