using Moq;
using ProductApi.Controllers;
using ProductApi.Interfaces;
using ProductApi.Models;
using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Responses;

namespace ProductApi.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);
        }

        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenFound()
        {
            // Arrange
            var product = new Product { ProductId = "123", Name = "Test" };
            _mockService.Setup(s => s.GetProductByIdAsync("123")).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById("123");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal("123", returnedProduct.ProductId);
            Assert.Equal("Test", returnedProduct.Name);
        }


        [Fact]
        public async Task CreateProduct_ReturnsCreatedApiResponse()
        {
            var product = new Product { ProductId = "456", Name = "Created" };
            _mockService.Setup(s => s.CreateProductAsync(It.IsAny<BaseProductDto>())).ReturnsAsync(product);

            var result = await _controller.CreateProduct(new BaseProductDto());

            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);

            var response = Assert.IsType<ApiResponse<Product>>(createdResult.Value);
            Assert.Equal("Product created successfully.", response.Message);
            Assert.Equal("456", response.Data?.ProductId);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsApiResponse_WhenSuccessful()
        {
            var updated = new Product { ProductId = "789", Name = "Updated" };
            _mockService.Setup(s => s.UpdateProductAsync("789", It.IsAny<BaseProductDto>())).ReturnsAsync(updated);

            var result = await _controller.UpdateProduct("789", new BaseProductDto());

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Product>>(okResult.Value);
            Assert.Equal("Product updated successfully.", response.Message);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsApiResponse_WhenDeleted()
        {
            _mockService.Setup(s => s.DeleteProductAsync("123")).ReturnsAsync(true);

            var result = await _controller.DeleteProduct("123");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(okResult.Value);
            Assert.Equal("Product deleted successfully.", response.Message);
        }

        [Fact]
        public async Task DecrementStock_ReturnsBadRequest_WhenFails()
        {
            _mockService.Setup(s => s.DecrementStockAsync("222", 5)).ReturnsAsync(false);

            var result = await _controller.DecrementStock("222", 5);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badResult.Value);
            Assert.Equal(400, response.StatusCode);
        }

        [Fact]
        public async Task AddToStock_ReturnsNotFound_WhenFails()
        {
            _mockService.Setup(s => s.AddToStockAsync("333", 10)).ReturnsAsync(false);

            var result = await _controller.AddToStock("333", 10);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.Equal(404, response.StatusCode);
        }
    }
}
