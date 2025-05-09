﻿using CSharpApp.Application.Products.Queries;
using MediatR;

namespace CSharpApp.Application.Products.Handlers
{
    public class GetProductHandler : IRequestHandler<GetProductQuery, Result<Product>>
    {
        private readonly IProductsService _productsService;
        public GetProductHandler(IProductsService productsService)
        {
            _productsService = productsService;
        }
        public async Task<Result<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productsService.GetProduct(request.ProductId);
            return product;
        }
    }
}
