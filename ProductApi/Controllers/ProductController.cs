using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Interfaces;
using ProductApi.Models;
using ProductApi.Responses;

namespace ProductApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }

        // GET: /api/products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameters query)
        {
            var (items, totalCount, totalPages) = await _service.GetAllProductAsync(query);

            if (items == null || !items.Any())
            {
                var emptyResponse = new ApiPagedResponse<Product>(
                    statusCode: 200,
                    message: "No products found.",
                    data: new List<Product>(),
                    page: query.Page,
                    pageSize: query.PageSize,
                    totalCount: 0,
                    totalPages: 0
                );
                return Ok(emptyResponse);
            }

            return Ok(new
            {
                data = items,
                page = query.Page,
                pageSize = query.PageSize,
                totalCount = totalCount,
                totalPages = totalPages
            });
        }


        // GET: /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _service.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // POST: /api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] BaseProductDto productDto)
        {
            var product = await _service.CreateProductAsync(productDto);
            var response = new ApiResponse<Product>(
                statusCode: 201,
                message: "Product created successfully.",
                data: product
            );

            return StatusCode(201, response);
        }

        // PUT: /api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] BaseProductDto productDto)
        {
            var product = await _service.UpdateProductAsync(id, productDto);
            if (product == null)
                return NotFound();

            var response = new ApiResponse<Product>(
                statusCode: 200,
                message: "Product updated successfully.",
                data: product
            );

            return Ok(response);
        }

        // PATCH: /api/products/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchProduct(string id, [FromBody] ProductPatchDto patchDto)
        {
            var product = await _service.PatchProductAsync(id, patchDto);
            if (product == null)
                return NotFound();

            var response = new ApiResponse<Product>(
                statusCode: 200,
                message: "Product updated successfully.",
                data: product
            );

            return Ok(response);
        }

        // DELETE: /api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var deleted = await _service.DeleteProductAsync(id);
            if (!deleted)
                return NotFound();

            var response = new ApiResponse<object>(
                statusCode: 200,
                message: "Product deleted successfully."
            );

            return Ok(response);
        }

        // PUT: /api/products/decrement-stock/{id}/{quantity}
        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStock(string id, int quantity)
        {
            var success = await _service.DecrementStockAsync(id, quantity);
            if (!success)
                return BadRequest(new ApiResponse<object>(400, "Insufficient stock or product not found."));

            var response = new ApiResponse<object>(
                statusCode: 200,
                message: "Stock decremented successfully."
            );

            return Ok(response);
        }

        // PUT: /api/products/add-to-stock/{id}/{quantity}
        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddToStock(string id, int quantity)
        {
            var success = await _service.AddToStockAsync(id, quantity);
            if (!success)
                return NotFound(new ApiResponse<object>(404, $"Product with ID {id} not found."));

            var response = new ApiResponse<object>(
                statusCode: 200,
                message: "Stock added successfully."
            );

            return Ok(response);
        }
    }
}
