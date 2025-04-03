using CSharpApp.Application.Products;
using Polly;
using Polly.Extensions.Http;

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


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

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

app.Run();