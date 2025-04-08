using CSharpApp.Application.Products.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Handlers
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<Product>>
    {
        private readonly IProductsService _productsService;
        public CreateProductHandler(IProductsService productService)
        {
            _productsService = productService;
        }
        public async Task<Result<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productsService.CreateProduct(request.CreateProductDto);
            return product;
        }
    }
}
