using EShopWebApp.Core.Services;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace TestingEShopWebApp.UnitTests
{
	[TestFixture]
	public class CartServiceTest
	{
		private ApplicationDbContext _context;
		private CartService _service;
		private Mock<IHttpContextAccessor> mockHttpContextAccessor;
		private Guid productId1 = Guid.NewGuid();
		private string sessionId = Guid.NewGuid().ToString();
		private Guid frontPhotoId = Guid.NewGuid();
		private ApplicationUser user;

	  [SetUp]
		public void Setup()
		{
			// Use In-memory database for testing
			var options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDatabase")
				.Options;
			_context = new ApplicationDbContext(options);
			//Mock IHttpContextAccessor
			mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
			var context = new DefaultHttpContext();

			// Mock the cookies
			var cookies = new Mock<IRequestCookieCollection>();
			cookies.Setup(c => c["ShoppingCartSessionId"]).Returns(sessionId);
			context.Request.Cookies = cookies.Object;

			mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

			_service = new CartService(_context, mockHttpContextAccessor.Object);

			// Seed the in-memory database with test data
			var frontPhoto = new Photo { Id = frontPhotoId, Name = "NewName", Picture = new byte[0] };
			_context.Photos.Add(frontPhoto);
			_context.SaveChanges();

			var session = new ShoppingCartSession { SessionId = Guid.Parse(sessionId) };

			_context.ShoppingCartSessions.Add(session);
			_context.SaveChanges();

			user = new ApplicationUser
			{
				Id = Guid.NewGuid(),
				UserName = "TestUser",
				Email = "",
				FirstName = "Test",
				LastName = "User",

			};
			_context.Users.Add(user);
			_context.SaveChanges();

			

			var product = new Product
			{
				Id = productId1,
				Name = "Product 1",
				Description = "Description 1",
				Quantity = 10,
				Price = 100,
				FrontPhotoId = frontPhotoId,
				CategoryId = Guid.NewGuid(),
				BrandId = Guid.NewGuid(),
				CreatedOn = DateTime.UtcNow
			};
			_context.Products.Add(product);
			_context.SaveChanges();
		}

		[Test]
		public async Task TestAddProductToGuestCartAsync_AddProduct()
		{


			// Act
			var result = await _service.AddProductToGuestCartAsync(productId1);
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.IsNotNull(result);
			Assert.That(result.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].ProductId, Is.EqualTo(productId1));
			Assert.That(list[0].Quantity, Is.EqualTo(1));
			var result2 = await _service.AddProductToGuestCartAsync(productId1);
			Assert.That(result2.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].Quantity, Is.EqualTo(2));
		}
		[Test]
		public async Task TestAddProducttoGuestCartAsync_AddProductAndIncreaseQuantity()
		{
			// Act
			var result = await _service.AddProductToGuestCartAsync(productId1);
			var result2 = await _service.AddProductToGuestCartAsync(productId1);
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.That(result2.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].Quantity, Is.EqualTo(2));
		}
		[Test]
		public async Task TestRemoveProductFromGuestCartAsync_RemoveProduct()
		{
			// Act
			var result = await _service.AddProductToGuestCartAsync(productId1);
			await _service.RemoveGuestProduct(productId1);
			var list = _context.ShoppingCartItems.ToList();
			
			Assert.That(list.Count, Is.EqualTo(0));
		}
		[Test]
		public async Task TestRemoveProductFromGuestCartAsync_RemoveProductAndDecreaseQuantity()
		{
			// Act
			var result = await _service.AddProductToGuestCartAsync(productId1);
			var result2 = await _service.AddProductToGuestCartAsync(productId1);
			await _service.RemoveGuestProduct(productId1);
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.That(list[0].Quantity, Is.EqualTo(1));
		}
		[Test]
		public async Task TestAddProductToCartAsync_AddProduct()
		{
			// Act
			var result = await _service.AddProductToCartAsync(productId1,user.Id.ToString());
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.IsNotNull(result);
			Assert.That(result.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].ProductId, Is.EqualTo(productId1));
			Assert.That(list[0].Quantity, Is.EqualTo(1));
			var result2 = await _service.AddProductToCartAsync(productId1, user.Id.ToString());
			Assert.That(result2.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].Quantity, Is.EqualTo(2));
		}
		[Test]
		public async Task TestAddProductToCartAsync_AddProductAndIncreaseQuantity()
		{
			//Arrange
			var result = await _service.AddProductToCartAsync(productId1, user.Id.ToString());
			// Act

			var result2 = await _service.AddProductToCartAsync(productId1, user.Id.ToString());
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.That(result2.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].Quantity, Is.EqualTo(2));
		}
		[Test]
		public async Task GetGuestCartProductsAsync()
		{
			//Arrange
			var result = await _service.AddProductToGuestCartAsync(productId1);
			var list = _context.ShoppingCartItems.ToList();
			// Act

			var result2 = await _service.GetGuestCartProductsAsync(Guid.Parse(sessionId));
			// Assert
			Assert.That(result2.Count(), Is.EqualTo(1));
			Assert.That(list[0].ProductId, Is.EqualTo(productId1));
			Assert.That(list[0].Quantity, Is.EqualTo(1));
		}
		[Test]
		public async Task TestCreateShoppingCartSession()
		{
			// Act
			string sessionId = await _service.CreateShoppingCartSession();
			var list = await _context.ShoppingCartSessions.ToListAsync();
			//// Assert
			Assert.IsNotNull(sessionId);
			
			Assert.That(list.Count, Is.EqualTo(2));
			Assert.That(list[1].SessionId, Is.EqualTo(Guid.Parse(sessionId)));
		}
		[Test]
		public async Task TestGetCartItems()
		{
			//Arrange
			var res = await _service.AddProductToGuestCartAsync(productId1);
			// Act
			var result2 = await _service.GetCartItems();
			// Assert
			Assert.That(result2.Count(), Is.EqualTo(1));
		}
		[Test]
		public async Task TestRemoveShoppingCartItemsAsync()
		{
			//Arrange
			var res = await _service.AddProductToCartAsync(productId1, user.Id.ToString());
			// Act
			await _service.RemoveShoppingCartItemsAsync(productId1.ToString(), user.Id.ToString());
			//var list = _context.ShoppingCartItems.ToList();
			// Assert
			//Assert.That(list.Count, Is.EqualTo(0));
		}
		[Test]
		public async Task TestRemoveShoppingCartItemsAsync_TestWithGuest()
		{
			//Arrange
			var res = await _service.AddProductToGuestCartAsync(productId1);
			// Act
			await _service.RemoveShoppingCartItemsAsync(productId1.ToString(), null);
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.That(list.Count, Is.EqualTo(0));
		}
		[Test]
		public async Task TestRemoveProduct()
		{
			//Arrange
			var res = await _service.AddProductToCartAsync(productId1, user.Id.ToString());
			// Act
			await _service.RemoveProduct(productId1, user.Id.ToString());
			var list = _context.ShoppingCartItems.ToList();
			// Assert
			Assert.That(list.Count, Is.EqualTo(0));
		}

		[TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}

}
