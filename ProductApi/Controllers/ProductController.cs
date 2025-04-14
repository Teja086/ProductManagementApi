using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Interfaces;
using ProductApi.Models;

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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        // GET: /api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _service.GetByIdAsync(id);
            return product is null ? NotFound() : Ok(product);
        }

        // POST: /api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] BaseProductDto productDto)
        {
            var created = await _service.CreateAsync(productDto);
            return CreatedAtAction(nameof(GetProductById), new { id = created.ProductId }, created);
        }

        // PUT: /api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] BaseProductDto productDto)
        {
            var updated = await _service.UpdateAsync(id, productDto);
            return updated is null ? NotFound() : Ok(updated);
        }

        // DELETE: /api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? Ok() : NotFound();
        }

        // PUT: /api/products/decrement-stock/{id}/{quantity}
        [HttpPut("decrement-stock/{id}/{quantity}")]
        public async Task<IActionResult> DecrementStock(string id, int quantity)
        {
            var success = await _service.DecrementStockAsync(id, quantity);
            return success ? Ok() : BadRequest("Insufficient stock or product not found");
        }

        // PUT: /api/products/add-to-stock/{id}/{quantity}
        [HttpPut("add-to-stock/{id}/{quantity}")]
        public async Task<IActionResult> AddToStock(string id, int quantity)
        {
            var success = await _service.AddToStockAsync(id, quantity);
            return success ? Ok() : NotFound();
        }
    }
}
