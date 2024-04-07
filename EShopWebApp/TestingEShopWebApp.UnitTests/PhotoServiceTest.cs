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
		Assert.IsNull(result);
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
	[TearDown]
	public void TearDown()
	{
		_context.Database.EnsureDeleted();
		_context.Dispose();
	}

}
