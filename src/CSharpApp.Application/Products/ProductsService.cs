using CSharpApp.Application.Constants;
using CSharpApp.Application.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    #region Properties
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;
    #endregion

    #region Constructor
    public ProductsService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<ProductsService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }
    #endregion

    #region Public Methods
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
                products = res!;
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

        return products!;
    }
    public async Task<Product> GetProduct(int id)
    {
        Product product = new();

        try
        {
            string productMethod = _restApiSettings.Products + "/" + (id.ToString() ?? string.Empty);
            var response = await _httpClient.GetAsync(productMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Product>(content);
                product = res!;
            }
            else
            {
                _logger.LogError($"Failed to retrieve product, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in ProductsService.GetProduct: {ex.Message}");
        }

        return product;
    }
    public async Task<Product> CreateProduct(CreateProductRequest request)
    {
        await CreateProductValidation(request);

        Product product = new();

        try
        {
            var jsonContent = JsonSerializer.Serialize(request);
            var contentRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string productMethod = _restApiSettings.Products;
            var response = await _httpClient.PostAsync(productMethod, contentRequest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Product>(content);
                product = res!;
            }
            else
            {
                _logger.LogError($"Failed to Create Product, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in ProductsService.CreateProduct: {ex.Message}");
        }

        return product;
    }
    #endregion

    #region Private Methods
    private async Task CreateProductValidation(CreateProductRequest request)
    {
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            foreach (var failure in validationResult.Errors)
            {
                if(failure.PropertyName.ToLower() == Properties.Title)
                {
                    throw new ArgumentException(ExceptionMessages.GeneralArgumentException, failure.PropertyName.ToLower());
                }

                _logger.LogError($" {LoggerMessages.LoggerValidationFail} {failure.ErrorMessage}");
            }
        }
    }
    #endregion
}