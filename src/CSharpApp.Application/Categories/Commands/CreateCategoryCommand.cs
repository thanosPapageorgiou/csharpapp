using MediatR;

namespace CSharpApp.Application.Categories.Commands
{
    public record CreateCategoryCommand(CreateCategory CreateCategoryDto): IRequest<Result<Category>>;
}
