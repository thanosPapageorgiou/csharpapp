using CSharpApp.Application.Categories;
using CSharpApp.Application.Products;
using CSharpApp.Core.Dtos;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Polly.Extensions.Http;
using System.ComponentModel.Design;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

string productApiBaseAddress = builder.Configuration.GetValue<string>("RestApiSettings:BaseUrl") ?? string.Empty;

var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(12));

builder.Services.AddHttpClient<IProductsService, ProductsService>(client =>
{
    client.BaseAddress = new Uri(productApiBaseAddress);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(timeoutPolicy);

builder.Services.AddHttpClient<ICategoriesService, CategoriesService>(client =>
{
    client.BaseAddress = new Uri(productApiBaseAddress);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(timeoutPolicy);

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(productApiBaseAddress);
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(timeoutPolicy);


//string redis = builder.Configuration.GetValue<string>("RestApiSettings:Redis") ?? string.Empty;
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration.GetConnectionString(redis);
//    options.InstanceName = "Token_";
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var versionedEndpointRouteBuilder = app.NewVersionedApi();


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


app.Run();