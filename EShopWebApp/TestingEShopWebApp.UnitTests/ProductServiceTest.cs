using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services;
using EShopWebApp.Core.Services.ServiceModels;
using EShopWebApp.Core.ViewModels.ImageViewModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using EShopWebApp.Core.ViewModels.ProductViewModels.Enums;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestingEShopWebApp.UnitTests
{
	public class ProductServiceTest
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
			//await _productService.CreateAsync(mockFile.Object, productCreateViewModel);

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
		[Test]
		public async Task DeleteAsync_MarksProductAsDeleted()
		{
			// Arrange
			var testId = productId1; // Use one of the product IDs from your test data

			// Act
			await _productService.DeleteAsync(testId);

			// Assert
			var product = await _context.Products.FindAsync(testId);
			Assert.That(product, Is.Not.Null);
			Assert.That(product.IsDeleted, Is.True);
		}
		[Test]
		public async Task EditAsync_UpdatesProduct()
		{
			// Arrange
			var testId = productId1; // Use one of the product IDs from your test data
			var mockFile = new Mock<IFormFile>();
			var editProductModel = new ProductEditViewModel
			{
				Name = "Updated Product",
				Description = "Updated Description",
				Price = 15.0m,
				StockQuantity = 10,
				CategoryId = Guid.NewGuid().ToString(),
				BrandId = Guid.NewGuid().ToString()
			};

			// Act
			await _productService.EditAsync(mockFile.Object, testId, editProductModel);

			// Assert
			var product = await _context.Products.FindAsync(testId);
			Assert.That(product, Is.Not.Null);
			Assert.That(product.Name, Is.EqualTo(editProductModel.Name));
			Assert.That(product.Description, Is.EqualTo(editProductModel.Description));
			Assert.That(product.Price, Is.EqualTo(editProductModel.Price));
			Assert.That(product.Quantity, Is.EqualTo(editProductModel.StockQuantity));
			Assert.That(product.CategoryId, Is.EqualTo(Guid.Parse(editProductModel.CategoryId)));
			Assert.That(product.BrandId, Is.EqualTo(Guid.Parse(editProductModel.BrandId)));
		}
		[Test]
		public async Task GetAllFilteredAndPagedAsync_ReturnsFilteredAndPagedProducts()
		{
			// Arrange
			var productsQueryModel = new AllProductsQueryModel
			{
				Brand = "Brand 1",
				Category = "Category 1",
				SearchTerm = "Product 1",
				ProductSorting = ProductSorting.Newest,
				CurrentPage = 1,
				PageSize = 1
			};

			// Act
			var result = await _productService.GetAllFilteredAndPagedAsync(productsQueryModel);

			// Assert
			Assert.That(result, Is.TypeOf<AllProductsFilteredAndPagedServiceModel>());
			Assert.That(result.TotalProducts, Is.EqualTo(1));
			Assert.That(result.Products, Has.Exactly(1).Items);
			var product = result.Products.First();
			Assert.That(product.Name, Is.EqualTo("Product 1"));
		}
		[Test]
		public async Task GetLastThreeAddedAsync_ReturnsLastThreeAddedProducts()
		{
			// Act
			var result = await _productService.GetLastThreeAddedAsync();
				List<ProductAllViewModel> products = result.ToList();

			// Assert
			Assert.That(result, Is.TypeOf<List<ProductAllViewModel>>());
			Assert.That(result.Count, Is.EqualTo(2));
			
			Assert.That(products[1].Name, Is.EqualTo("Product 2")); // Assuming "Product 2" is the second last added product
			Assert.That(products[0].Name, Is.EqualTo("Product 1")); // Assuming "Product 1" is the third last added product
		}





		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}
}
