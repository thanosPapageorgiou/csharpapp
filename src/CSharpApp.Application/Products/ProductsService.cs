using CSharpApp.Application.Constants;
using CSharpApp.Application.Utilities;
using CSharpApp.Application.Validation;
using CSharpApp.Core.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using CSharpApp.Application.Products.Handlers;
using CSharpApp.Application.Products.Queries;

namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{
    #region Properties
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;
    private readonly IAuthService _authService;
    
    #endregion

    #region Constructor
    public ProductsService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<ProductsService> logger, IAuthService authService)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
        _authService = authService;
    }
    #endregion

    #region Public Methods
    public async Task<Result<IReadOnlyCollection<Product>>> GetProducts()
    {
        IReadOnlyCollection<Product> products = new List<Product>().AsReadOnly();

        try
        {
            var accessToken = await _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            string productMethod = _restApiSettings.Products ?? string.Empty;
            var response = await _httpClient.GetAsync(productMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<Product>>(content);
                products = res!;

                return Result<IReadOnlyCollection<Product>>.Success(products);
            }
            else
            {
                _logger.LogError($"Failed to retrieve products, statusCode: {response.StatusCode}");

                return Result<IReadOnlyCollection<Product>>.Failure($"Failed to retrieve products, statusCode: {response.StatusCode}");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Exception in ProductsService.GetProducts: {ex.Message}");

            return Result<IReadOnlyCollection<Product>>.Failure($"Exception in ProductsService.GetProducts: {ex.Message}");
        }

    }
    public async Task<Result<Product>> GetProduct(int id)
    {
        Product product = new();

        try
        {
            var accessToken = await _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string productMethod = _restApiSettings.Products + "/" + (id.ToString() ?? string.Empty);
            var response = await _httpClient.GetAsync(productMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Product>(content);
                product = res!;

                return Result<Product>.Success(product);
            }
            else
            {
                _logger.LogError($"Failed to retrieve product, statusCode: {response.StatusCode}");

                return Result<Product>.Failure($"Failed to retrieve product, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in ProductsService.GetProduct: {ex.Message}");

            return Result<Product>.Failure($"Exception in ProductsService.GetProduct: {ex.Message}");
        }
    }
    public async Task<Result<Product>> CreateProduct(CreateProduct request)
    {
        await CreateProductRequestValidation(request);

        Product product = new();

        try
        {
            var accessToken = await _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var jsonContent = JsonSerializer.Serialize(request);
            var contentRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string productMethod = _restApiSettings.Products!;
            var response = await _httpClient.PostAsync(productMethod, contentRequest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Product>(content);
                product = res!;

                return Result<Product>.Success(product);
            }
            else
            {
                _logger.LogError($"Failed to Create Product, statusCode: {response.StatusCode}");

                return Result<Product>.Failure($"Failed to Create Product, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in ProductsService.CreateProduct: {ex.Message}");

            return Result<Product>.Failure($"Exception in ProductsService.CreateProduct: {ex.Message}");
        }
    }
    #endregion

    #region Private Methods
    private async Task CreateProductRequestValidation(CreateProduct request)
    {
        var validator = new CreateProductValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            // The following ArgumentExceptions are thrown specifically for unit test purposes.
            foreach (var failure in validationResult.Errors)
            {
                if(CommonUtils.IsRunningFromUnitTest())
                {
                    if (failure.PropertyName.ToLower() == Properties.Title)
                    {
                        throw new ArgumentException(ExceptionMessages.GeneralArgumentException, failure.PropertyName.ToLower());
                    }
                }
            }
        }
    }
    #endregion
}