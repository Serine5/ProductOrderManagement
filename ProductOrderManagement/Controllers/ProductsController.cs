using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductOrderManagement.Models;
using ServiceLayer.IServices;
using ServiceLayer.Models;

namespace ProductOrderManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService) =>
            _productService = productService;

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _productService.GetAllProductsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) 
                return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var createdProduct = await _productService.CreateProductAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid || id != request.Id) 
                return BadRequest(ModelState);

            var updatedProduct = await _productService.UpdateProductAsync(request);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductStock(UpdateProductStockRequest request)
        {
            var updatedProductStock = await _productService.UpdateProductStockAsync(request);
            return Ok(updatedProductStock);
        }
    }
}
