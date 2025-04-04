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
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
              //  Email = dto.Email,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Name = user.Username
            });
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var loggedInUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (userRole != "Admin" && loggedInUserId != id)
            {
                return Forbid("You are not authorized to view other users' information.");
            }

            var user = await _context.Users
                .Include(u => u.Visits)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound();

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Username,
                Visits = user.Visits.Select(v => new VisitDto
                {
                    Id = v.Id,
                    VisitDate = v.VisitDate
                }).ToList()
            };

            return Ok(userDto);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users.Include(u => u.Visits).ToListAsync();

            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Username,
                Visits = u.Visits.Select(v => new VisitDto
                {
                    Id = v.Id,
                    VisitDate= v.VisitDate
                }).ToList()
            });

            return Ok(userDtos);
        }
    }
}
