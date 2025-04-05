using CSharpApp.Application.Products;
using CSharpApp.Core.Dtos;
using CSharpApp.Core.Settings;
using CSharpApp.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CSharpApp.Tests.Products
{
    public class ProductsServiceTests
    {
        #region GetProducts Empty Records Should Be Fail
        [Fact]
        public async void GetProducts_EmptyRecords_ShouldFail()
        {
            //Arrange
            var mockProducts = new List<Product>
            {
                new() { Id = 1, Title = "Test Product 1", Price = 10 },
                new() { Id = 2, Title = "Test Product 2", Price = 11 },
                new() { Id = 3, Title = "Test Product 3", Price = 12 }
            };
            var json = JsonSerializer.Serialize(mockProducts);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "products" });
            var logger = new LoggerFactory().CreateLogger<ProductsService>();

            var service = new ProductsService(httpClient, settings, logger);

            //Act
            var actual = await service.GetProducts();

            //Assert
            Assert.True(actual.Count > 0);
        }
        #endregion

        #region GetSingleProduct Different ProductID Should Be Fail
        [Theory]
        [InlineData(1)] 
        [InlineData(2)]  
        [InlineData(3)] 
        public async void GetSingleProduct_Diff_ProductID_ShouldFail(int productId)
        {
            //Arrange
            var mockProduct = new Product { Id = productId, Title = "Test Product " + productId, Price = 12 };

            var json = JsonSerializer.Serialize(mockProduct);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "products" });
            var logger = new LoggerFactory().CreateLogger<ProductsService>();

            var service = new ProductsService(httpClient, settings, logger);


            //Act
            var actual = await service.GetProduct(productId);

            //Assert
            Assert.Equal(productId, actual.Id);
        }
        #endregion

        #region CreateProduct Different Title Should Be Fail
        [Theory]
        [InlineData("test-title-prod-1", 10, "test-descr-prod-1", 1, "https://placeimg.com/640/480/any")]
        [InlineData("test-title-prod-2", 20, "test-descr-prod-2", 2, "https://placeimg.com/640/480/another")]
        public async void CreateProduct_Diff_Title_ShouldFail(string title, decimal price, string description, int categoryId, string image)
        {
            //Arrange
            Random random = new();
            var mockProduct = new Product {
                Id = random.Next(),
                Title = title,
                Price = price,
                Description = description,
                Category = new Category { Id = categoryId, CreationAt = DateTime.Now, Name = "category_" + categoryId },
                Images = new List<string> { image },
                CreationAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var json = JsonSerializer.Serialize(mockProduct);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "products" });
            var logger = new LoggerFactory().CreateLogger<ProductsService>();

            var service = new ProductsService(httpClient, settings, logger);


            //Act
            var newProductRequest = new CreateProductRequest
            {
                Title = title,
                Price = price,
                Description = description,
                CategoryId = categoryId,
                Images = new List<string> { image }
            };
            var actual = await service.CreateProduct(newProductRequest);

            //Assert
            Assert.True(actual != null);
            Assert.Equal(title, actual.Title);
        }
        #endregion

        #region CreateProduct empty Title parameter Should Fail
        [Theory]
        [InlineData("", 10, "test-descr-prod-1", 1, "https://placeimg.com/640/480/any")]
        public async void CreateProduct_Empty_Title_Parameter_ShouldFail(string title, decimal price, string description, int categoryId, string image)
        {
            //Arrange
            Random random = new();
            var mockProduct = new Product
            {
                Id = random.Next(),
                Title = title,
                Price = price,
                Description = description,
                Category = new Category { Id = categoryId, CreationAt = DateTime.Now, Name = "category_" + categoryId },
                Images = new List<string> { image },
                CreationAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var json = JsonSerializer.Serialize(mockProduct);
            var httpClient = MockHttpClientFactory.CreateHttpClient(json);

            var settings = Options.Create(new RestApiSettings { Products = "products" });
            var logger = new LoggerFactory().CreateLogger<ProductsService>();

            var service = new ProductsService(httpClient, settings, logger);


            //Act
            var newProductRequest = new CreateProductRequest
            {
                Title = title,
                Price = price,
                Description = description,
                CategoryId = categoryId,
                Images = new List<string> { image }
            };

             await Assert.ThrowsAsync<ArgumentException>(title, async () => await service.CreateProduct(newProductRequest));
        }
        #endregion
    }
}