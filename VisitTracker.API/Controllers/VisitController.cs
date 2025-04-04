using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitTracker.API.Data;
using VisitTracker.API.Dtos;
using VisitTracker.API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace VisitTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VisitsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Standard")]
        public async Task<ActionResult<VisitDto>> CreateVisit(CreateVisitDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var store = await _context.Stores.FindAsync(dto.StoreId);
            if (store == null)
                return NotFound("Store not found.");

            var visit = new Visit
            {
                VisitDate = DateTime.UtcNow,
                UserId = userId,
                Store = store,
                Status = "In Progress"
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            var visitDto = new VisitDto
            {
                Id = visit.Id,
                VisitDate = visit.VisitDate,
                Status = visit.Status,
                StoreId = store.Id,
                UserId = userId
            };

            return CreatedAtAction(nameof(GetVisitById), new { id = visit.Id }, visitDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitDto>> GetVisitById(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Store)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (visit == null)
                return NotFound();

            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userRole == "Standard" && visit.UserId != userId)
                return Forbid("You can only access your own visits.");

            return Ok(new VisitDto
            {
                Id = visit.Id,
                VisitDate = visit.VisitDate,
                Status = visit.Status,
                StoreId = visit.Store.Id,
                UserId = visit.UserId
            });
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetVisits(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortBy = "VisitDate",
            [FromQuery] string? sortOrder = "desc",
            [FromQuery] string? status = null,
            [FromQuery] int? storeId = null)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var query = _context.Visits
                .Include(v => v.User)
                .Include(v => v.Store)
                .Include(v => v.Photos)
                    .ThenInclude(p => p.Product)
                .AsQueryable();

            if (userRole == "Standard")
                query = query.Where(v => v.UserId == userId);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(v => v.Status == status);

            if (storeId.HasValue)
                query = query.Where(v => v.StoreId == storeId);

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortOrder?.ToLower() == "asc")
                    query = query.OrderBy(v => EF.Property<object>(v, sortBy));
                else
                    query = query.OrderByDescending(v => EF.Property<object>(v, sortBy));
            }

            var totalCount = await query.CountAsync();

            var visits = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => new
                {
                    v.Id,
                    v.VisitDate,
                    v.Status,
                    Store = new { v.Store.Id, v.Store.Name, v.Store.Location },
                    User = new { v.User.Id, v.User.Username },
                    Photos = v.Photos.Select(p => new
                    {
                        p.Id,
                        p.Base64Image,
                        p.UploadedAt,
                        Product = new { p.Product.Id, p.Product.Name, p.Product.Category }
                    })
                })
                .ToListAsync();

            return Ok(new
            {
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Data = visits
            });
        }

        [HttpPut("{visitId}/complete")]
        [Authorize(Roles = "Standard")]
        public async Task<IActionResult> CompleteVisit(int visitId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var visit = await _context.Visits.FirstOrDefaultAsync(v => v.Id == visitId && v.UserId == userId);
            if (visit == null)
                return NotFound("Visit not found or you're not authorized.");

            visit.Status = "Completed";
            await _context.SaveChangesAsync();

            return Ok("Visit marked as completed.");
        }
    }
}
