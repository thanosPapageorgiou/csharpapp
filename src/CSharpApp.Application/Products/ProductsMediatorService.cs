using MediatR;
using CSharpApp.Application.Products.Queries;
using CSharpApp.Application.Products.Commands;

namespace CSharpApp.Application.Products;

public class ProductsMediatorService : IProductsMediatorService
{
    #region Properties
    private readonly IMediator _mediator;
    
    #endregion

    #region Constructor
    public ProductsMediatorService(IMediator mediator)
    {
        _mediator = mediator;
    }
    #endregion

    #region Public Methods
    public async Task<Result<IReadOnlyCollection<Product>>> GetProducts()
    {
        var products = await _mediator.Send(new GetProductListQuery());

        return products;
    }
    public async Task<Result<Product>> GetProduct(int productId)
    {
        var product = await _mediator.Send(new GetProductQuery(productId));
        return product;
    }
    public async Task<Result<Product>> CreateProduct(CreateProduct request)
    {
        var product = await _mediator.Send(new CreateProductCommand(request));
        return product;
    }
    #endregion
}