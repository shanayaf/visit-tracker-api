using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitTracker.API.Data;
using VisitTracker.API.Models;
using VisitTracker.API.Dtos;
using Microsoft.AspNetCore.Authorization;



namespace VisitTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoresController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            return await _context.Stores.ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Store>> CreateStore(StoreDto storeDto)
        {
            var store = new Store
            {
                Name = storeDto.Name,
                Location = storeDto.Location,
                CreatedAt = DateTime.UtcNow
            };

            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStores), new { id = store.Id }, store);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStore(int id, StoreDto storeDto)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            store.Name = storeDto.Name;
            store.Location = storeDto.Location;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);
            if (store == null) return NotFound();

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
