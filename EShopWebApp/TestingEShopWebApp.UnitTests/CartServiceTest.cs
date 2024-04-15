using EShopWebApp.Core.Contracts;
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

			var cart = new ShoppingCart
			{
                Id = Guid.NewGuid(),
                UserId = user.Id,
                ShoppingCartItems = new List<ShoppingCartItem>
				{
                    new ShoppingCartItem
					{
                        Id = Guid.NewGuid(),
                        ProductId = productId1,
                        Quantity = 1
                    }
                }
            };
			_context.ShoppingCarts.Add(cart);
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
			Assert.That(list[0].Quantity, Is.EqualTo(1));
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
			Assert.That(list[0].Quantity, Is.EqualTo(1));
			Assert.That(list[0].ProductId, Is.EqualTo(productId1));
			

		}
		[Test]
		public async Task TestRemoveProductFromGuestCartAsync_RemoveProduct()
		{
			// Act
			var result = await _service.AddProductToGuestCartAsync(productId1);
			await _service.RemoveGuestProduct(productId1);
			var list = _context.ShoppingCartItems.ToList();
			
			Assert.That(list.Count, Is.EqualTo(1));
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
			Assert.That(list[0].Quantity, Is.EqualTo(2));
			var result2 = await _service.AddProductToCartAsync(productId1, user.Id.ToString());
			Assert.That(result2.ShoppingCartItems.Count(), Is.EqualTo(1));
			Assert.That(list[0].Quantity, Is.EqualTo(3));
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
			Assert.That(list[0].Quantity, Is.EqualTo(3));
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
			Assert.That(list.Count, Is.EqualTo(1));
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
			Assert.That(list.Count, Is.EqualTo(1));
		}
        [Test]
        public async Task GetCartAsync_WithValidUserId_ShouldReturnCartViewModel()
        {
            // Arrange

            // Act
            var result = await _service.GetCartAsync(user.Id.ToString());

            // Assert
            Assert.IsNotNull(result);
			
			Assert.That(result.ShoppingCartItems.Count(), Is.EqualTo(1));

			
          
        }
        [Test]
        public async Task GetCartAsync_WithInvalidUserId_ShouldReturnEmptyCartViewModel()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            // Act
            var result = await _service.GetCartAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Id, Is.EqualTo(Guid.Empty));
            Assert.IsEmpty(result.ShoppingCartItems);
        }
        [Test]
        public async Task GetGuestCartAsync_WithExistingSessionId_ShouldReturnCartViewModel()
        {
            // Arrange
           

            // Add shopping cart data to the in-memory database
            await _context.ShoppingCarts.AddAsync(new ShoppingCart
            {
                Id = Guid.NewGuid(),
                SessionId = Guid.Parse(sessionId),
                ShoppingCartItems = { new ShoppingCartItem { Id = Guid.NewGuid(), ProductId = productId1, Quantity = 1 } }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetGuestCartAsync(sessionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ShoppingCartItems);
            Assert.That(result.ShoppingCartItems.Count(), Is.EqualTo(1));
            Assert.That(result.SessionId, Is.EqualTo(Guid.Parse(sessionId)));
            Assert.That(result.ShoppingCartItems.First().ProductId, Is.EqualTo(productId1));
        }

        [Test]
        public async Task GetGuestCartAsync_WithNonExistingSessionId_ShouldReturnEmptyCartViewModel()
        {
            // Arrange
            var nonExistingSessionId = Guid.NewGuid().ToString();
			

            // Act
            var result = await _service.GetGuestCartAsync(nonExistingSessionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result.ShoppingCartItems);
			
            
        }
        [Test]
        public async Task AddCartItemToUserCart_WithNewUserAndProduct_ShouldCreateNewCartAndCartItem()
        {
            // Arrange
            
            
            var quantity = 2;

            // Act
            await _service.AddCartItemToUserCart(productId1, quantity, user.Id.ToString());

            // Assert
            var cart = await _context.ShoppingCarts.Include(c => c.ShoppingCartItems)
                                                   .FirstOrDefaultAsync(c => c.UserId == user.Id);

            Assert.NotNull(cart);
            Assert.That(cart.ShoppingCartItems.Count, Is.EqualTo(1));
            Assert.That(cart.ShoppingCartItems.First().ProductId, Is.EqualTo(productId1));
            Assert.That(cart.ShoppingCartItems.First().Quantity, Is.EqualTo(1+ quantity));
        }

        [Test]
        public async Task AddCartItemToUserCart_WithExistingUserAndProduct_ShouldIncreaseQuantity()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var productId = Guid.NewGuid();
            var initialQuantity = 1;
            var additionalQuantity = 2;

            // Add an existing cart item to the database
            await _context.ShoppingCarts.AddAsync(new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                ShoppingCartItems = { new ShoppingCartItem { Id = Guid.NewGuid(), ProductId = productId, Quantity = initialQuantity } }
            });
            await _context.SaveChangesAsync();

            // Act
            await _service.AddCartItemToUserCart(productId, additionalQuantity, userId);

            // Assert
            var cart = await _context.ShoppingCarts.Include(c => c.ShoppingCartItems)
                                                   .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));

            Assert.NotNull(cart);
            Assert.That(cart.ShoppingCartItems.Count, Is.EqualTo(1));
            Assert.That(cart.ShoppingCartItems.First().ProductId, Is.EqualTo(productId));
            Assert.That(cart.ShoppingCartItems.First().Quantity, Is.EqualTo(initialQuantity + additionalQuantity));
        }

        [Test]
        public async Task AddCartItemToUserCart_WithNewUserButExistingProduct_ShouldCreateNewCartItem()
        {
            // Arrange
            
            var quantity = 2;

            // Add an existing product to the database
           

            // Act
            await _service.AddCartItemToUserCart(productId1, quantity, user.Id.ToString());

            // Assert
            var cart = await _context.ShoppingCarts.Include(c => c.ShoppingCartItems)
                                                   .FirstOrDefaultAsync(c => c.UserId == user.Id);

            Assert.NotNull(cart);
            Assert.That(cart.ShoppingCartItems.Count, Is.EqualTo(1));
            Assert.That(cart.ShoppingCartItems.First().ProductId, Is.EqualTo(productId1));
            Assert.That(cart.ShoppingCartItems.First().Quantity, Is.EqualTo(1+quantity));
        }

        [TearDown]
		public void TearDown()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}

}
