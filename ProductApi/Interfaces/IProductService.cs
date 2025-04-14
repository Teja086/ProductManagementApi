using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(string id);
        Task<Product> CreateAsync(BaseProductDto product);
        Task<Product?> UpdateAsync(string id, BaseProductDto updatedProduct);
        Task<bool> DeleteAsync(string id);
        Task<bool> DecrementStockAsync(string id, int quantity);
        Task<bool> AddToStockAsync(string id, int quantity);
    }
}
