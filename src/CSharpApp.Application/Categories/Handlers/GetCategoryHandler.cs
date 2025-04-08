using CSharpApp.Application.Categories.Queries;
using MediatR;

namespace CSharpApp.Application.Categories.Handlers
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, Result<Category>>
    {
        private readonly ICategoriesService _categoriesService;
        public GetCategoryHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        public async Task<Result<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoriesService.GetCategory(request.CategoryId);
            return category;
        }
    }
}
