using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitTracker.API.Data;
using VisitTracker.API.Models;
using VisitTracker.API.Dtos;

namespace VisitTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PhotosController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Upload a photo to a visit
        [HttpPost("{visitId}")]
        [Authorize(Roles = "Standard")]
        public async Task<IActionResult> UploadPhoto(int visitId, CreatePhotoDto photoDto)
        {
            var visit = await _context.Visits.FindAsync(visitId);
            if (visit == null) return NotFound("Visit not found.");

            var product = await _context.Products.FindAsync(photoDto.ProductId);
            if (product == null) return NotFound("Product not found.");

            var photo = new Photo
            {
                VisitId = visitId,
                ProductId = photoDto.ProductId,
                Base64Image = photoDto.Base64Image,
                UploadedAt = DateTime.UtcNow
            };

            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();

            var photoDtoResponse = new PhotoDto
            {
                Id = photo.Id,
                VisitId = visitId,
                ProductId = photo.ProductId,
                Base64Image = photo.Base64Image,
                UploadedAt = photo.UploadedAt
            };

            return CreatedAtAction(nameof(GetPhotoById), new { id = photo.Id }, photoDtoResponse);
        }

        // ✅ Get a photo by ID
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PhotoDto>> GetPhotoById(int id)
        {
            var photo = await _context.Photos.FindAsync(id);
            if (photo == null) return NotFound();

            var photoDto = new PhotoDto
            {
                Id = photo.Id,
                VisitId = photo.VisitId,
                ProductId = photo.ProductId,
                Base64Image = photo.Base64Image,
                UploadedAt = photo.UploadedAt
            };

            return Ok(photoDto);
        }
    }
}
