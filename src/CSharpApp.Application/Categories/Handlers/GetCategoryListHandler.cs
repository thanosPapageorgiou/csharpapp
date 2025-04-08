using CSharpApp.Application.Categories.Queries;
using MediatR;

namespace CSharpApp.Application.Categories.Handlers
{
    public class GetCategoryListHandler : IRequestHandler<GetCategoryListQuery, Result<IReadOnlyCollection<Category>>>
    {
        private readonly ICategoriesService _categoriesService;
        public GetCategoryListHandler(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        public async Task<Result<IReadOnlyCollection<Category>>> Handle(GetCategoryListQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoriesService.GetCategories();
            return categories;
        }
    }
}
