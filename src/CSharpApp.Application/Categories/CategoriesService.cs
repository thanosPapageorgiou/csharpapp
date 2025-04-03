using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CSharpApp.Application.Categories;

public class CategoriesService : ICategoriesService
{
    private readonly HttpClient _httpClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<CategoriesService> _logger;

    public CategoriesService(HttpClient httpClient, IOptions<RestApiSettings> restApiSettings, ILogger<CategoriesService> logger)
    {
        _httpClient = httpClient;
        _restApiSettings = restApiSettings.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Category>> GetCategories()
    {
        IReadOnlyCollection<Category> categories = new List<Category>().AsReadOnly();

        try
        {
            string categoryMethod = _restApiSettings.Categories ?? string.Empty;
            var response = await _httpClient.GetAsync(categoryMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<List<Category>>(content);
                categories = res!;
            }
            else
            {
                _logger.LogError($"Failed to retrieve Categories, statusCode: {response.StatusCode}");
            }
        }
        catch(Exception ex)
        {
            _logger.LogError($"Exception in CategoriesService.GetCategories: {ex.Message}");
        }

        return categories!;
    }
    public async Task<Category> GetCategory(int id)
    {
        Category category = new();

        try
        {
            string categoryMethod = _restApiSettings.Categories + "/" + (id.ToString() ?? string.Empty);
            var response = await _httpClient.GetAsync(categoryMethod);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);
                category = res!;
            }
            else
            {
                _logger.LogError($"Failed to retrieve Category, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in CategoriesService.GetCategory: {ex.Message}");
        }

        return category;
    }
    public async Task<Category> CreateCategory(CreateCategoryRequest request)
    {
        Category category = new();

        try
        {
            var jsonContent = JsonSerializer.Serialize(request);
            var contentRequest = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            string categoryMethod = _restApiSettings.Categories;
            var response = await _httpClient.PostAsync(categoryMethod, contentRequest);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var res = JsonSerializer.Deserialize<Category>(content);
                category = res!;
            }
            else
            {
                _logger.LogError($"Failed to Create Category, statusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception in CategoriesService.CreateCategory: {ex.Message}");
        }

        return category;
    }
}