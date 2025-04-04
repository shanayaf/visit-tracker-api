using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using VisitTracker.API.Controllers;
using VisitTracker.API.Models;
using VisitTracker.API.Data;
using VisitTracker.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VisitTracker.Tests
{
    public class PhotosControllerTests
    {
       private AppDbContext GetInMemoryDbContext()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // ✅ Step 2 handled here
        .EnableSensitiveDataLogging() // optional but helps debugging
        .Options;

    var context = new AppDbContext(options);

    // ✅ Add a mock store
    var store = new Store { Id = 1, Name = "Test Store" };
    context.Stores.Add(store);

    // ✅ Add a mock product linked to that store
    var product = new Product
    {
        Id = 1,
        Name = "Test Product",
        StoreId = store.Id // required FK
    };
    context.Products.Add(product);

    // ✅ Add a visit
    var visit = new Visit
    {
        Id = 1,
        UserId = 1,
        VisitDate = DateTime.UtcNow
    };
    context.Visits.Add(visit);

    context.SaveChanges();
    return context;
}


        [Fact]
        public async Task UploadPhoto_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new PhotosController(context);
            var photoDto = new CreatePhotoDto
            {
                ProductId = 1,
                Base64Image = "Today"
            };

            // Act
            var result = await controller.UploadPhoto(1, photoDto);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<PhotoDto>(createdResult.Value);
            Assert.Equal(photoDto.ProductId, returnValue.ProductId);
        }

        [Fact]
        public async Task GetPhotoById_ValidId_ReturnsPhoto()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var photo = new Photo
            {
                VisitId = 1,
                ProductId = 1,
                Base64Image = "MyTestImage",
                UploadedAt = DateTime.UtcNow
            };
            context.Photos.Add(photo);
            context.SaveChanges();

            var controller = new PhotosController(context);

            // Act
            var result = await controller.GetPhotoById(photo.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<PhotoDto>(okResult.Value);
            Assert.Equal(photo.Base64Image, returnValue.Base64Image);
        }
    }
}
