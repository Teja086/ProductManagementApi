using ProductApi.Services;
using ProductApi.Data;
using ProductApi.Models;
using Microsoft.EntityFrameworkCore;
using ProductApi.DTOs;
using ProductApi.Exceptions;

namespace ProductApi.Tests.Services
{
    public class ProductServiceTests
    {
        private ProductDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(databaseName: dbName) // Unique name per test
                .Options;

            return new ProductDbContext(options);
        }


        [Fact]
        public async Task CreateAsync_ShouldAddProduct()
        {
            // Arrange
            var context = GetInMemoryDbContext(nameof(CreateAsync_ShouldAddProduct));
            var service = new ProductService(context);
            var product = new BaseProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 99.99M,
                StockAvailable = 10
            };

            // Act
            var result = await service.CreateAsync(product);

            // Assert
            Assert.NotNull(result.ProductId);
            Assert.Equal("Test Product", result.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_IfExists()
        {
            var context = GetInMemoryDbContext(nameof(GetByIdAsync_ShouldReturnProduct_IfExists));
            var service = new ProductService(context);

            var product = new Product
            {
                ProductId = "123456",
                Name = "Test",
                Description = "Test Desc",
                Price = 10,
                StockAvailable = 5
            };
            context.Products.Add(product);
            context.SaveChanges();

            var result = await service.GetByIdAsync("123456");

            Assert.NotNull(result);
            Assert.Equal("Test", result?.Name);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_IfNotExists()
        {
            var context = GetInMemoryDbContext(nameof(GetByIdAsync_ShouldReturnNull_IfNotExists));
            var service = new ProductService(context);

            await Assert.ThrowsAsync<NotFoundException>(() => service.GetByIdAsync("notfound"));
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenExists()
        {
            var context = GetInMemoryDbContext(nameof(UpdateAsync_ShouldUpdateProduct_WhenExists));
            var service = new ProductService(context);

            context.Products.Add(new Product
            {
                ProductId = "111111",
                Name = "Old Name",
                Description = "Old Desc",
                Price = 50,
                StockAvailable = 5
            });
            context.SaveChanges();

            var updatedDto = new BaseProductDto
            {
                Name = "New Name",
                Description = "New Desc",
                Price = 99.99m,
                StockAvailable = 10
            };

            // Act
            var result = await service.UpdateAsync("111111", updatedDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New Name", result!.Name);
        }


        [Fact]
        public async Task AddToStockAsync_ShouldIncreaseStock_WhenProductExists()
        {
            var context = GetInMemoryDbContext(nameof(AddToStockAsync_ShouldIncreaseStock_WhenProductExists));
            var service = new ProductService(context);

            context.Products.Add(new Product
            {
                ProductId = "222222",
                Name = "Item",
                StockAvailable = 5
            });
            context.SaveChanges();

            var result = await service.AddToStockAsync("222222", 3);

            Assert.True(result);
            var updated = await service.GetByIdAsync("222222");
            Assert.Equal(8, updated?.StockAvailable);
        }


        [Fact]
        public async Task DecrementStockAsync_ShouldThrow_WhenStockIsInsufficient()
        {
            var context = GetInMemoryDbContext(nameof(DecrementStockAsync_ShouldThrow_WhenStockIsInsufficient));
            var service = new ProductService(context);

            context.Products.Add(new Product
            {
                ProductId = "111111",
                Name = "Item",
                StockAvailable = 1,
                Price = 50
            });
            context.SaveChanges();

            await Assert.ThrowsAsync<BadRequestException>(() => service.DecrementStockAsync("111111", 5));
        }

        [Fact]
        public async Task DecrementStockAsync_ShouldSucceed_WhenStockIsEnough()
        {
            var context = GetInMemoryDbContext(nameof(DecrementStockAsync_ShouldSucceed_WhenStockIsEnough));
            var service = new ProductService(context);

            context.Products.Add(new Product
            {
                ProductId = "222222",
                Name = "Item",
                StockAvailable = 10,
                Price = 100
            });
            context.SaveChanges();

            var result = await service.DecrementStockAsync("222222", 3);

            Assert.True(result);
            var updated = await service.GetByIdAsync("222222");
            Assert.Equal(7, updated?.StockAvailable);
        }
    }
}
