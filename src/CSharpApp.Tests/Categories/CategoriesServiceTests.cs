using CSharpApp.Application.Categories;
using CSharpApp.Application.Products;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using CSharpApp.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CSharpApp.Tests.Categories
{
    public class CategoriesServiceTests
    {
        #region GetCategories_test
        [Fact]
        public async void GetCategories_test()
        {
            //Arrange
            var mockCategories = new List<Category>
            {
                new() { Id = 1, Name = "Test Category 1", CreationAt = DateTime.Now, UpdatedAt = DateTime.Now, Image = "" },
                new() { Id = 2, Name = "Test Category 2", CreationAt = DateTime.Now, UpdatedAt = DateTime.Now, Image = "https://placeimg.com/640/480/any" },
                new() { Id = 3, Name = "Test Category 3", CreationAt = DateTime.Now, UpdatedAt = DateTime.Now, Image = "" }
            };
            var json = JsonSerializer.Serialize(mockCategories);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "categories" });
            var logger = new LoggerFactory().CreateLogger<CategoriesService>();

            var service = new CategoriesService(httpClient, settings, logger);

            //Act
            var actual = await service.GetCategories();

            var imageCounter = actual.Select(s => s.Image.Equals("")).Count();

          Assert.True(actual.Count() == 3);
        }
        #endregion

        #region GetSinglemockCategory_test
        [Theory]
        [InlineData(1)] 
        [InlineData(2)]  
        [InlineData(3)] 
        public async void GetSingleCategory_test(int categoryId)
        {
            //Arrange
            var mockCategory = new Category { Id = categoryId, Name = "Test Category " + categoryId };

            var json = JsonSerializer.Serialize(mockCategory);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "categories" });
            var logger = new LoggerFactory().CreateLogger<CategoriesService>();

            var service = new CategoriesService(httpClient, settings, logger);


            //Act
            var actual = await service.GetCategory(categoryId);

            //Assert
            Assert.Equal(categoryId, actual.Id);
        }
        #endregion

        #region CreateCategory_test
        [Theory]
        [InlineData("test-title-category-1", "https://placeimg.com/640/480/any")]
        [InlineData("test-title-category-2", "https://placeimg.com/640/480/another")]
        public async void CreateProduct_test(string name, string image)
        {
            //Arrange
            Random random = new();
            var mockCategory = new Category {
                Id = random.Next(),
                Name = name,
                Image = image,
                CreationAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var json = JsonSerializer.Serialize(mockCategory);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "categories" });
            var logger = new LoggerFactory().CreateLogger<CategoriesService>();

            var service = new CategoriesService(httpClient, settings, logger);


            //Act
            var newCategoryRequest = new CreateCategoryRequest
            {
                Name = name,
                Image = image 
            };
            var actual = await service.CreateCategory(newCategoryRequest);

            //Assert
            Assert.True(actual != null);
            Assert.Equal(name, actual.Name);
        }
        #endregion
    }
}