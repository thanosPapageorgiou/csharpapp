using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<ProductsService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        IReadOnlyCollection<Product> products = new List<Product>().AsReadOnly();

        try
        {
            string productMethod = _restApiSettings.Products ?? string.Empty;
            var response = await _httpClient.GetAsync(productMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<Product>>(content);
                products = res.AsReadOnly();
            }
            else
            {
                _logger.LogError($"Failed to retrieve products, statusCode: {response.StatusCode}");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Exception in ProductsService.GetProducts: {ex.Message}");
        }

        return products;
    }
}