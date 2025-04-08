using CSharpApp.Application.Categories.Commands;
using MediatR;

namespace CSharpApp.Application.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<Category>>
    {
        private readonly ICategoriesService _categoriesService;
        public CreateCategoryHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoriesService.CreateCategory(request.CreateCategoryDto);
            return category;
        }
    }
}
