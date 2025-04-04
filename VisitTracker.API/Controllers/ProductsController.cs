using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitTracker.API.Data;
using VisitTracker.API.Models;
using VisitTracker.API.Dtos;
using System.Security.Claims;

namespace VisitTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Admin sees all, Standard must provide storeId
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] int? storeId)
        {
            var role = User.Claims.FirstOrDefault(c => c.Type.Contains("role"))?.Value;

            if (role == "Admin")
            {
                return await _context.Products.ToListAsync();
            }

            if (role == "Standard")
            {
                if (storeId == null)
                    return BadRequest("Standard users must provide storeId.");

                var products = await _context.Products
                    .Where(p => p.StoreId == storeId)
                    .ToListAsync();

                return products;
            }

            return Forbid();
        }

        // ✅ Only Admins can create products
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> CreateProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Category = productDto.Category,
                StoreId = productDto.StoreId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }

        // ✅ Only Admins can update products
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = productDto.Name;
            product.Category = productDto.Category;
            product.StoreId = productDto.StoreId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ✅ Only Admins can delete products
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
