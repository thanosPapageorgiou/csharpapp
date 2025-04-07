using CSharpApp.Application.Categories;
using CSharpApp.Application.Products;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Interfaces;
using CSharpApp.Core.Settings;
using CSharpApp.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace CSharpApp.Tests.Categories
{
    public class CategoriesServiceTests
    {
        #region GetCategories Should Return Records
        [Theory]
        [InlineData(3)]
        [InlineData(0)]
        public async void GetCategories_ShouldReturnRecords(int expected)
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
            //var cacheMock = new Mock<IDistributedCache>();
            var authService = new Mock<IAuthService>();

     

            var service = new CategoriesService(httpClient, settings, logger, authService.Object);

            //Act
            var actual = await service.GetCategories();


            Assert.Equal(expected, actual.Data.Count);
        }
        #endregion

        #region GetSingCategory Different CategoryID Should Fail
        [Theory]
        [InlineData(1, 1)] 
        [InlineData(2, 100)]  
        [InlineData(3, 3)] 
        public async void GetSingleCategory_Diff_CategoryID_ShouldFail(int categoryId, int expected)
        {
            //Arrange
            var mockCategory = new Category { Id = categoryId, Name = $"Test Category { categoryId }" };

            var json = JsonSerializer.Serialize(mockCategory);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "categories" });
            var logger = new LoggerFactory().CreateLogger<CategoriesService>();
            var authService = new Mock<IAuthService>();

            var service = new CategoriesService(httpClient, settings, logger, authService.Object);

            //Act
            var actual = await service.GetCategory(categoryId);

            //Assert
            Assert.Equal(expected, actual.Data.Id);
        }
        #endregion

        #region CreateCategory If Return Different Record Should Be Fail
        [Theory]
        [InlineData("test-title-category-1", "https://placeimg.com/640/480/any", "test-title-category-1")]
        [InlineData("test-title-category-2", "https://placeimg.com/640/480/another", "test-title-category-100")]
        public async void CreateCategory_Return_Diff_Record_ShouldFail(string name, string image, string expected)
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
            var authService = new Mock<IAuthService>();
            //var cacheMock = new Mock<IDistributedCache>();

            var service = new CategoriesService(httpClient, settings, logger, authService.Object);


            //Act
            var newCategoryRequest = new CreateCategoryRequest
            {
                Name = name,
                Image = image 
            };
            var actual = await service.CreateCategory(newCategoryRequest);

            //Assert
            Assert.True(actual != null);
            Assert.Equal(expected, actual.Data.Name);
        }
        #endregion

        #region CreateCategory If Has Empty -Name OR Image- Parameter Should Throw ArgumentException
        [Theory]
        [InlineData("", "https://placeimg.com/640/480/any")]
        [InlineData("test-title-category-2", "")]
        public async void CreateProduct_Empty_Parameter_Should_ThrowArgumentException(string name, string image)
        {
            //Arrange
            Random random = new();
            var mockCategory = new Category
            {
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
            var authService = new Mock<IAuthService>();

            var service = new CategoriesService(httpClient, settings, logger, authService.Object);


            //Act
            var newCategoryRequest = new CreateCategoryRequest
            {
                Name = name,
                Image = image
            };

            //Assert
            var prop = name == string.Empty ? "name" : "image";
            await Assert.ThrowsAsync<ArgumentException>(prop, async () => await service.CreateCategory(newCategoryRequest));
        }
        #endregion
    }
}