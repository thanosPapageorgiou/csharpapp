using CSharpApp.Core.Dtos;

namespace CSharpApp.Api.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MapProjectEndpoints(this WebApplication app)
        {
            var versionedEndpointRouteBuilder = app.NewVersionedApi();

            #region Products Endpoints
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproducts", async (IProductsService productsService) =>
            {
                var products = await productsService.GetProducts();
                if (products.IsSuccess)
                {
                    return Results.Ok(products.Data); 
                }
                else
                {
                    return Results.StatusCode(products.StatusCode);
                }
            })
            .WithName("GetProducts")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (IProductsService productsService, int id) =>
            {
                var product = await productsService.GetProduct(id);
                if (product.IsSuccess)
                {
                    return Results.Ok(product.Data);
                }
                else
                {
                    return Results.StatusCode(product.StatusCode);
                }
            })
                .WithName("GetProduct")
                .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (IProductsService productsService, CreateProductRequest request) =>
            {
                var newProduct = await productsService.CreateProduct(request);
                if (newProduct.IsSuccess)
                {
                    return Results.Ok(newProduct.Data);
                }
                else
                {
                    return Results.StatusCode(newProduct.StatusCode);
                }
            })
                .WithName("CreateProduct")
                .HasApiVersion(1.0);
            #endregion

            #region Categories Endpoints
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
            {
                var categories = await categoriesService.GetCategories();
                if (categories.IsSuccess)
                {
                    return Results.Ok(categories.Data);
                }
                else
                {
                    return Results.StatusCode(categories.StatusCode);
                }
            })
                .WithName("GetCategories")
                .HasApiVersion(1.0);


            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategory/{id:int}", async (ICategoriesService categoriesService, int id) =>
            {
                var category = await categoriesService.GetCategory(id);
                if (category.IsSuccess)
                {
                    return Results.Ok(category.Data);
                }
                else
                {
                    return Results.StatusCode(category.StatusCode);
                }
            })
                .WithName("GetCategory")
                .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory", async (ICategoriesService categoriesService, CreateCategoryRequest request) =>
            {
                var newCategory = await categoriesService.CreateCategory(request);
                if (newCategory.IsSuccess)
                {
                    return Results.Ok(newCategory.Data);
                }
                else
                {
                    return Results.StatusCode(newCategory.StatusCode);
                }
            })
                .WithName("CreateCategory")
                .HasApiVersion(1.0);
            #endregion

            return app;
        }
    }
}
