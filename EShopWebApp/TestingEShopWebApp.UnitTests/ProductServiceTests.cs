using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using EShopWebApp.Core.ViewModels.ImageViewModels;
using Moq;
using Microsoft.AspNetCore.Http;

namespace TestingEShopWebApp.UnitTests
{
	public class ProductServiceTests
	{
		private ApplicationDbContext _context;
		private Mock<IPhotoService> _photoServiceMock;
		private ProductService _productService;
		Guid productId1 ;
		Guid productId2 ;
		Guid productId3 ;
		
		[SetUp]
		public void Setup()
		{
			// Use a fresh in-memory database for each test
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new ApplicationDbContext(options);

			_photoServiceMock = new Mock<IPhotoService>();

			// Setup _photoServiceMock to return a specific result when GetPhotoById is called
			_photoServiceMock.Setup(service => service.GetPhotoById(It.IsAny<Guid>()))
				.ReturnsAsync(new PhotoViewModel { Name = "Test", Picture = new byte[0] });
			_photoServiceMock.Setup(service => service.CreateImage(It.IsAny<Microsoft.AspNetCore.Http.IFormFile>(), It.IsAny<string>()))
				.Returns(new PhotoViewModel { Name = "Test", Picture = new byte[0] });
			_photoServiceMock.Setup(service => service.DownloadPhotoAsync(It.IsAny<Guid>()))
				.Returns(Task.CompletedTask);
			

			_productService = new ProductService(_context, _photoServiceMock.Object);

			// Seed the in-memory database with test data

			 productId1 = Guid.NewGuid();
			 productId2 = Guid.NewGuid();
			 productId3 = Guid.NewGuid();
			var frontPhoto = new Photo { Id = Guid.NewGuid(), Name="NewName", Picture = new byte[0] };
			_context.Photos.Add(frontPhoto);
			_context.SaveChanges();
			var categoryId = Guid.NewGuid();
			var brandId = Guid.NewGuid();
			var category = new Category { Id = categoryId, Name = "Category 1" };
			var brand = new Brand { Id = brandId, Name = "Brand 1" };
			_context.Categories.Add(category);
			_context.Brands.Add(brand);
			_context.Products.AddRange(new[]
			{
				new Product { Id = productId1, Name = "Product 1", Price = 10.0m, IsDeleted = false,CategoryId= categoryId,Description="New Description",BrandId = brandId,FrontPhoto = frontPhoto },
				new Product { Id = productId2, Name = "Product 2", Price = 20.0m, IsDeleted = false,CategoryId= categoryId,Description="New Description",BrandId = brandId,FrontPhoto = frontPhoto },
				new Product { Id = productId3, Name = "Product 3", Price = 30.0m, IsDeleted = true, CategoryId= categoryId, Description="New Description", BrandId = brandId, FrontPhoto = frontPhoto}
			});
			_context.SaveChanges();
		}

		[Test]
		public async Task GetAllAsync_ReturnsNonDeletedProducts()
		{
			// Act
			var result = await _productService.GetAllAsync();

			// Assert
			Assert.That(result, Is.TypeOf<List<ProductAllViewModel>>());
			// TODO: Add more assertions based on your setup
			Assert.That(result.Count, Is.EqualTo(2));
			Assert.That(result.Any());
			Assert.That(result.Any(p => p.Id == productId1.ToString()));
		}

		[Test]
		public async Task GetByIdAsync_ReturnsCorrectProduct()
		{
			// Arrange
			

			// Act
			var result = await _productService.GetByIdAsync(productId1);

			// Assert
			Assert.That(result, Is.TypeOf<ProductAllViewModel>());
			Assert.That(result.Id, Is.EqualTo(productId1.ToString()));
		}
		[Test]
		public async Task CreateAsync_AddsNewProduct()
		{
			// Arrange
			var mockFile = new Mock<IFormFile>();
			mockFile.Setup(file => file.FileName).Returns("test.jpg");
			mockFile.Setup(file => file.Length).Returns(1);
			var productCreateViewModel = new ProductCreateViewModel
			{
				Name = "Test Product",
				Description = "Test Description",
				Price = 10.0m,
				StockQuantity = 5,
				CategoryId = Guid.NewGuid().ToString(),
				BrandId = Guid.NewGuid().ToString()
			};

			// Act
			await _productService.CreateAsync(mockFile.Object, productCreateViewModel);

			// Assert
			//Assert.That(_context.Products, Has.Exactly(1).Items);
			var product = _context.Products.Where(p => p.Name == "Test Product").First();
			Assert.That(product.Name, Is.EqualTo(productCreateViewModel.Name));
			Assert.That(product.Description, Is.EqualTo(productCreateViewModel.Description));
			Assert.That(product.Price, Is.EqualTo(productCreateViewModel.Price));
			Assert.That(product.Quantity, Is.EqualTo(productCreateViewModel.StockQuantity));
			Assert.That(product.CategoryId, Is.EqualTo(Guid.Parse(productCreateViewModel.CategoryId)));
			Assert.That(product.BrandId, Is.EqualTo(Guid.Parse(productCreateViewModel.BrandId)));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}
}
