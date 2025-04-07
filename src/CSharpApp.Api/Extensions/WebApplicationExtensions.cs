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
                return products;
            })
            .WithName("GetProducts")
            .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getproduct/{id:int}", async (IProductsService productsService, int id) =>
            {
                var product = await productsService.GetProduct(id);
                return product;
            })
                .WithName("GetProduct")
                .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createproduct", async (IProductsService productsService, CreateProductRequest request) =>
            {
                var newProduct = await productsService.CreateProduct(request);
                return newProduct;
            })
                .WithName("CreateProduct")
                .HasApiVersion(1.0);
            #endregion

            #region Categories Endpoints
            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategories", async (ICategoriesService categoriesService) =>
            {
                var products = await categoriesService.GetCategories();
                return products;
            })
                .WithName("GetCategories")
                .HasApiVersion(1.0);


            versionedEndpointRouteBuilder.MapGet("api/v{version:apiVersion}/getcategory/{id:int}", async (ICategoriesService categoriesService, int id) =>
            {
                var category = await categoriesService.GetCategory(id);
                return category;
            })
                .WithName("GetCategory")
                .HasApiVersion(1.0);

            versionedEndpointRouteBuilder.MapPost("api/v{version:apiVersion}/createcategory", async (ICategoriesService categoriesService, CreateCategoryRequest request) =>
            {
                var newCategory = await categoriesService.CreateCategory(request);
                return newCategory;
            })
                .WithName("CreateCategory")
                .HasApiVersion(1.0);
            #endregion

            return app;
        }
    }
}
