using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.DTOs;
using ProductApi.Exceptions;
using ProductApi.Interfaces;
using ProductApi.Models;
using ProductApi.Utilities;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductDbContext _context;

        public ProductService(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            return product;
        }

        public async Task<Product> CreateAsync(BaseProductDto productDto)
        {
            var product = new Product
            {
                ProductId = ProductIdGenerator.GenerateUniqueId(),
                Name = productDto.Name,
                Description = productDto.Description,
                StockAvailable = productDto.StockAvailable,
                Price = productDto.Price,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> UpdateAsync(string id, BaseProductDto updatedProductDto)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            // Only update fields if new value is not null or empty/default
            if (!string.IsNullOrWhiteSpace(updatedProductDto.Name))
                existing.Name = updatedProductDto.Name;

            if (!string.IsNullOrWhiteSpace(updatedProductDto.Description))
                existing.Description = updatedProductDto.Description;

            if (updatedProductDto.Price > 0)
                existing.Price = updatedProductDto.Price;

            existing.StockAvailable = updatedProductDto.StockAvailable;

            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = "system";

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DecrementStockAsync(string id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            if (product.StockAvailable < quantity)
                throw new BadRequestException("Insufficient stock.");

            product.StockAvailable -= quantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddToStockAsync(string id, int quantity)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            product.StockAvailable += quantity;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
