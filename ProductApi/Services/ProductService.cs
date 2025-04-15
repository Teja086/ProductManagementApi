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

        public async Task<(List<Product>, int)> GetAllProductAsync(ProductQueryParameters query)
        {
            var productsQuery = _context.Products.AsQueryable();

            var totalCount = await productsQuery.CountAsync();

            var items = await productsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            return product;
        }

        public async Task<Product> CreateProductAsync(BaseProductDto productDto)
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

        public async Task<Product?> UpdateProductAsync(string id, BaseProductDto updatedProductDto)
        {
            var existing = await _context.Products.FindAsync(id);
            if (existing == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            existing.Name = updatedProductDto.Name;
            existing.Description = updatedProductDto.Description;
            existing.Price = updatedProductDto.Price;
            existing.StockAvailable = updatedProductDto.StockAvailable;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.UpdatedBy = "system";

            await _context.SaveChangesAsync();
            return existing;
        }

        // Services/ProductService.cs
        public async Task<Product?> PatchProductAsync(string id, ProductPatchDto patchDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                throw new NotFoundException($"Product with ID {id} not found.");

            if (patchDto.Name != null)
                product.Name = patchDto.Name;

            if (patchDto.Description != null)
                product.Description = patchDto.Description;

            if (patchDto.Price.HasValue)
                product.Price = patchDto.Price.Value;

            if (patchDto.StockAvailable.HasValue)
                product.StockAvailable = patchDto.StockAvailable.Value;

            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = "system";

            await _context.SaveChangesAsync();
            return product;
        }


        public async Task<bool> DeleteProductAsync(string id)
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
