using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Interfaces
{
    public interface IProductService
    {
        //Task<IEnumerable<Product>> GetAllProductAsync();
        Task<(List<Product> Items, int TotalCount, int TotalPages)> GetAllProductAsync(ProductQueryParameters query);
        Task<Product?> GetProductByIdAsync(string id);
        Task<Product> CreateProductAsync(BaseProductDto product);
        Task<Product?> UpdateProductAsync(string id, BaseProductDto updatedProduct);
        Task<Product?> PatchProductAsync(string id, ProductPatchDto patchDto);
        Task<bool> DeleteProductAsync(string id);
        Task<bool> DecrementStockAsync(string id, int quantity);
        Task<bool> AddToStockAsync(string id, int quantity);
    }
}
