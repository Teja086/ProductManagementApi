using Moq;
using ProductApi.Controllers;
using ProductApi.Interfaces;
using ProductApi.Models;
using ProductApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;

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
        public async Task GetById_ReturnsOk_WithProduct()
        {
            var product = new Product { ProductId = "123456", Name = "Test" };
            _mockService.Setup(s => s.GetByIdAsync("123456")).ReturnsAsync(product);

            var result = await _controller.GetProductById("123456");

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(product, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.GetByIdAsync("notfound"))
                        .ThrowsAsync(new NotFoundException("Not found"));

            await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetProductById("notfound"));
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WithProduct()
        {
            var product = new Product { ProductId = "987654", Name = "Created" };
            _mockService.Setup(s => s.CreateAsync(It.IsAny<BaseProductDto>())).ReturnsAsync(product);

            var result = await _controller.CreateProduct(new BaseProductDto
            {
                Name = "Created",
                Description = "Test Desc",
                Price = 10,
                StockAvailable = 5
            });

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("987654", ((Product)created.Value!).ProductId);
        }

        [Fact]
        public async Task Update_ReturnsOk_WhenSuccessful()
        {
            var updated = new Product { ProductId = "111111", Name = "Updated" };
            _mockService.Setup(s => s.UpdateAsync("111111", It.IsAny<BaseProductDto>())).ReturnsAsync(updated);

            var result = await _controller.UpdateProduct("111111", new BaseProductDto());

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Updated", ((Product)ok.Value!).Name);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenProductDoesNotExist()
        {
            _mockService.Setup(s => s.UpdateAsync("999999", It.IsAny<BaseProductDto>()))
                        .ReturnsAsync((Product?)null);

            var result = await _controller.UpdateProduct("999999", new BaseProductDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DecrementStock_ReturnsOk_WhenSuccessful()
        {
            _mockService.Setup(s => s.DecrementStockAsync("111111", 5)).ReturnsAsync(true);

            var result = await _controller.DecrementStock("111111", 5);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DecrementStock_ReturnsBadRequest_WhenFails()
        {
            _mockService.Setup(s => s.DecrementStockAsync("111111", 5)).ReturnsAsync(false);

            var result = await _controller.DecrementStock("111111", 5);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Insufficient stock or product not found", bad.Value);
        }

        [Fact]
        public async Task AddToStock_ReturnsOk_WhenSuccessful()
        {
            _mockService.Setup(s => s.AddToStockAsync("123456", 10)).ReturnsAsync(true);

            var result = await _controller.AddToStock("123456", 10);

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task AddToStock_ReturnsNotFound_WhenFails()
        {
            _mockService.Setup(s => s.AddToStockAsync("notfound", 10)).ReturnsAsync(false);

            var result = await _controller.AddToStock("notfound", 10);

            Assert.IsType<NotFoundResult>(result);
        }

    }
}
