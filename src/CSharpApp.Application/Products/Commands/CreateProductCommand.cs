using MediatR;

namespace CSharpApp.Application.Products.Commands
{
    public record CreateProductCommand(CreateProduct CreateProductDto): IRequest<Result<Product>>;
}
