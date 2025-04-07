using CSharpApp.Application.Constants;
using CSharpApp.Application.Utilities;
using CSharpApp.Application.Validation;
using CSharpApp.Core.Dtos;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CSharpApp.Application.Categories;

public class CategoriesService : ICategoriesService
{
    #region Properties
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CategoriesService> _logger;
    private readonly IAuthService _authService;
    #endregion

    #region Constructor
    public CategoriesService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CategoriesService> logger, IAuthService authService)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
        _authService = authService;
    }
    #endregion

    #region Public Methods
    public async Task<Result<IReadOnlyCollection<Category>>> GetCategories()
    {
        IReadOnlyCollection<Category> categories = new List<Category>().AsReadOnly();

        try
        {
            var accessToken = await _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string categoryMethod = _restApiSettings.Categories ?? string.Empty;
            var response = await _httpClient.GetAsync(categoryMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<Category>>(content);
                categories = res!;

                return Result<IReadOnlyCollection<Category>>.Success(categories);
            }
            else
            {
                _logger.LogError($"Failed to retrieve Categories, statusCode: {response.StatusCode}");

                return Result<IReadOnlyCollection<Category>>.Failure($"Failed to retrieve Categories, statusCode: {response.StatusCode}");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Exception in CategoriesService.GetCategories: {ex.Message}");

            return Result<IReadOnlyCollection<Category>>.Failure($"Exception in CategoriesService.GetCategories: {ex.Message}");
        }
    }
    public async Task<Result<Category>> GetCategory(int categoryId)
    {
        Category category = new();

        try
        {
            var accessToken = await _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            string categoryMethod = _restApiSettings.Categories + "/" + (categoryId.ToString() ?? string.Empty);
            var response = await _httpClient.GetAsync(categoryMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);
                category = res!;

                return Result<Category>.Success(category);
            }
            else
            {
                _logger.LogError($"Failed to retrieve Category, statusCode: {response.StatusCode}");

                return Result<Category>.Failure($"Failed to retrieve Category, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in CategoriesService.GetCategory: {ex.Message}");

            return Result<Category>.Failure($"Exception in CategoriesService.GetCategory: {ex.Message}");
        }
    }
    public async Task<Result<Category>> CreateCategory(CreateCategoryRequest request)
    {
        await CreateCategoryValidation(request);

        Category category = new();

        try
        {
            var accessToken = await _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var jsonContent = JsonSerializer.Serialize(request);
            var contentRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string categoryMethod = _restApiSettings.Categories!;
            var response = await _httpClient.PostAsync(categoryMethod, contentRequest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);
                category = res!;

                return Result<Category>.Success(category);
            }
            else
            {
                _logger.LogError($"Failed to Create Category, statusCode: {response.StatusCode}");

                return Result<Category>.Failure($"Failed to Create Category, statusCode: {response.StatusCode}");

            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in CategoriesService.CreateCategory: {ex.Message}");

            return Result<Category>.Failure($"Exception in CategoriesService.CreateCategory: {ex.Message}");
        }
    }
    #endregion

    #region Private Methods
    private async Task CreateCategoryValidation(CreateCategoryRequest request)
    {
        var validator = new CreateCategoryRequestValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            // The following ArgumentExceptions are thrown specifically for unit test purposes.
            foreach (var failure in validationResult.Errors)
            {
                if (CommonUtils.IsRunningFromUnitTest())
                {
                    throw new ArgumentException(ExceptionMessages.GeneralArgumentException, failure.PropertyName.ToLower());
                }
            }
        }
    }
    #endregion

}