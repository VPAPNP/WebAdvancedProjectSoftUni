using EShopWebApp.Core.Services;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;

[TestFixture]
public class PhotoServiceTests
{
	private ApplicationDbContext _context;
	private PhotoService _service;

	[SetUp]
	public void Setup()
	{
		// Use In-memory database for testing
		var options = new DbContextOptionsBuilder<ApplicationDbContext>()
			.UseInMemoryDatabase(databaseName: "TestDatabase")
			.Options;
		_context = new ApplicationDbContext(options);

		_service = new PhotoService(_context);
	}

	[Test]
	public void CreateImage_ShouldCreateImage()
	{
		// Arrange
		var fileMock = new Mock<IFormFile>();
		var content = "Hello World from a Fake File";
		var fileName = "test.jpg";
		var ms = new MemoryStream();
		var writer = new StreamWriter(ms);
		writer.Write(content);
		writer.Flush();
		ms.Position = 0;
		fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
		fileMock.Setup(_ => _.FileName).Returns(fileName);
		fileMock.Setup(_ => _.Length).Returns(ms.Length);

		// Act
		var result = _service.CreateImage(fileMock.Object, fileName);

		// Assert
		Assert.IsNotNull(result);
		Assert.That(result.Name, Is.EqualTo(fileName));
	}
	[Test]
	public async Task GetPhotoByName_ShouldReturnCorrectPhoto()
	{
		// Arrange
		var photoName = "test.jpg";
		var photo = new Photo
		{
			Name = photoName,
			Picture = Encoding.ASCII.GetBytes("Fake Image Data")
		};
		_context.Photos.Add(photo);
		await _context.SaveChangesAsync();

		// Act
		var result = await _service.GetPhotoByName(photoName);

		// Assert
		Assert.IsNotNull(result);
		Assert.That(result.Name, Is.EqualTo(photoName));
		Assert.That(result.Picture, Is.EqualTo(photo.Picture));
	}
	[Test]
	public async Task DeletePhotoAsync_ShouldRemovePhoto()
	{
		// Arrange
		var photoId = Guid.NewGuid();
		var photo = new Photo
		{
			Id = photoId,
			Name = "test.jpg",
			Picture = Encoding.ASCII.GetBytes("Fake Image Data")
		};
		_context.Photos.Add(photo);
		await _context.SaveChangesAsync();

		// Act
		await _service.DeletePhotoAsync(photoId);

		// Assert
		var result = await _context.Photos.FindAsync(photoId);
		bool isDeleted = result.IsDeleted;
		Assert.IsTrue(isDeleted);
	}
	[Test]
	public async Task GetPhotoById_ShouldReturnCorrectPhoto()
	{
		// Arrange
		var photoId = Guid.NewGuid();
		var photo = new Photo
		{
			Id = photoId,
			Name = "test.jpg",
			Picture = Encoding.ASCII.GetBytes("Fake Image Data")
		};
		_context.Photos.Add(photo);
		await _context.SaveChangesAsync();

		// Act
		var result = await _service.GetPhotoById(photoId);

		// Assert
		Assert.IsNotNull(result);
		Assert.That(result.Name, Is.EqualTo(photo.Name));
		Assert.That(result.Picture, Is.EqualTo(photo.Picture));
	}
    [Test]
    public async Task GetPhotoByProductId_WithExistingProductId_ShouldReturnPhotos()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var photoId1 = Guid.NewGuid();
        var photoId2 = Guid.NewGuid();

        // Add photos with the specified product ID to the database
        await _context.Photos.AddRangeAsync(
            new Photo { Id = photoId1, Name = "Photo 1", Picture = new byte[] { 0x01, 0x02, 0x03 }, ProductId = productId },
            new Photo { Id = photoId2, Name = "Photo 2", Picture = new byte[] { 0x04, 0x05, 0x06 }, ProductId = productId }
        );
        await _context.SaveChangesAsync();

        // Act
        var photosViewModel = await _service.GetPhotoByProductId(productId);

        // Assert
        Assert.IsNotNull(photosViewModel);
        Assert.That(photosViewModel.Count, Is.EqualTo(2));
        Assert.IsTrue(photosViewModel.Any(p => p.Name == "Photo 1" && p.Picture.SequenceEqual(new byte[] { 0x01, 0x02, 0x03 })));
        Assert.IsTrue(photosViewModel.Any(p => p.Name == "Photo 2" && p.Picture.SequenceEqual(new byte[] { 0x04, 0x05, 0x06 })));
    }

    [Test]
    public async Task GetPhotoByProductId_WithNonExistingProductId_ShouldReturnEmptyList()
    {
        // Arrange
        var nonExistingProductId = Guid.NewGuid();

        // Act
        var photosViewModel = await _service.GetPhotoByProductId(nonExistingProductId);

        // Assert
        Assert.IsNotNull(photosViewModel);
        Assert.IsEmpty(photosViewModel);
    }
    [TearDown]
	public void TearDown()
	{
		_context.Database.EnsureDeleted();
		_context.Dispose();
	}

}
