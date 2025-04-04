using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using VisitTracker.API.Controllers;
using VisitTracker.API.Data;
using VisitTracker.API.Dtos;
using VisitTracker.API.Models;
using Xunit;

namespace VisitTracker.Tests
{
    public class VisitsControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            // Seed a store and user
            context.Stores.Add(new Store { Id = 1, Name = "Test Store", Location = "Test City" });
            context.Users.Add(new User { Id = 1, Username = "testuser", Role = "Standard" });
            context.SaveChanges();

            return context;
        }

        private ClaimsPrincipal GetMockUser(int userId = 1, string role = "Standard")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            return new ClaimsPrincipal(identity);
        }

        [Fact]
        public async Task CreateVisit_ValidData_ReturnsCreatedResult()
        {
            var context = GetDbContext();
            var controller = new VisitsController(context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetMockUser() }
            };

            var dto = new CreateVisitDto { StoreId = 1 };
            var result = await controller.CreateVisit(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<VisitDto>(createdResult.Value);
            Assert.Equal(1, returnValue.StoreId);
        }

        [Fact]
        public async Task GetVisitById_ValidId_ReturnsVisit()
        {
            var context = GetDbContext();
            var visit = new Visit { Id = 1, VisitDate = DateTime.UtcNow, UserId = 1, StoreId = 1, Status = "In Progress" };
            context.Visits.Add(visit);
            context.SaveChanges();

            var controller = new VisitsController(context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetMockUser() }
            };

            var result = await controller.GetVisitById(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<VisitDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task CompleteVisit_ValidId_ChangesStatus()
        {
            var context = GetDbContext();
            context.Visits.Add(new Visit { Id = 2, VisitDate = DateTime.UtcNow, UserId = 1, StoreId = 1, Status = "In Progress" });
            context.SaveChanges();

            var controller = new VisitsController(context);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = GetMockUser() }
            };

            var result = await controller.CompleteVisit(2);
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Visit marked as completed.", okResult.Value);

            var visit = context.Visits.First(v => v.Id == 2);
            Assert.Equal("Completed", visit.Status);
        }
    }
}
