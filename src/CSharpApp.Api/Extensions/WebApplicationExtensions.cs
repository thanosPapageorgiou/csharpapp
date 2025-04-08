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

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (IProductsService productsService, CreateProduct request) =>
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

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory", async (ICategoriesService categoriesService, CreateCategory request) =>
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

            #region Mediator Products Endpoints
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproductsusingmediator", async (IProductsMediatorService productsMediatorService) =>
            {
                var products = await productsMediatorService.GetProducts();
                if (products.IsSuccess)
                {
                    return Results.Ok(products.Data);
                }
                else
                {
                    return Results.StatusCode(products.StatusCode);
                }
            })
           .WithName("GetProductsUsingMediator")
           .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproductusingmediator/{productId:int}", async (IProductsMediatorService productsMediatorService, int productId) =>
            {
                var product = await productsMediatorService.GetProduct(productId);
                if (product.IsSuccess)
                {
                    return Results.Ok(product.Data);
                }
                else
                {
                    return Results.StatusCode(product.StatusCode);
                }
            })
            .WithName("GetProductUsingMediator")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproductusingmediator", async (IProductsMediatorService productsMediatorService, CreateProduct request) =>
            {
                var newProduct = await productsMediatorService.CreateProduct(request);
                if (newProduct.IsSuccess)
                {
                    return Results.Ok(newProduct.Data);
                }
                else
                {
                    return Results.StatusCode(newProduct.StatusCode);
                }
            })
            .WithName("CreateProductUsingMediator")
            .HasApiVersion(1.0);
            #endregion

            #region Mediator Categories Endpoints
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategoriesusingmediator", async (ICategoriesMediatorService categoriesMediatorService) =>
            {
                var categories = await categoriesMediatorService.GetCategories();
                if (categories.IsSuccess)
                {
                    return Results.Ok(categories.Data);
                }
                else
                {
                    return Results.StatusCode(categories.StatusCode);
                }
            })
           .WithName("GetCategoriesUsingMediator")
           .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategoryusingmediator/{categoryId:int}", async (ICategoriesMediatorService categoriesMediatorService, int categoryId) =>
            {
                var category = await categoriesMediatorService.GetCategory(categoryId);
                if (category.IsSuccess)
                {
                    return Results.Ok(category.Data);
                }
                else
                {
                    return Results.StatusCode(category.StatusCode);
                }
            })
            .WithName("GetCategoryUsingMediator")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategoryusingmediator", async (ICategoriesMediatorService categoriesMediatorService, CreateCategory request) =>
            {
                var newCategory = await categoriesMediatorService.CreateCategory(request);
                if (newCategory.IsSuccess)
                {
                    return Results.Ok(newCategory.Data);
                }
                else
                {
                    return Results.StatusCode(newCategory.StatusCode);
                }
            })
            .WithName("CreateCategoryUsingMediator")
            .HasApiVersion(1.0);
            #endregion

            return app;
        }
    }
}
