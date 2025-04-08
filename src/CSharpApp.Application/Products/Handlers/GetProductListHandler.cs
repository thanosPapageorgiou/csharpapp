using CSharpApp.Application.Products.Queries;
using MediatR;

namespace CSharpApp.Application.Products.Handlers
{
    public class GetProductListHandler : IRequestHandler<GetProductListQuery, Result<IReadOnlyCollection<Product>>>
    {
        private readonly IProductsService _productsService;
        public GetProductListHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }
        public async Task<Result<IReadOnlyCollection<Product>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var products = await _productsService.GetProducts();
            return products;
        }
    }
}
