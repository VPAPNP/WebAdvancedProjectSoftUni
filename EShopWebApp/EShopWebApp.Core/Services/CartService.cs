using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{

    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public CartService(ApplicationDbContext context,
			IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            
        }
        

        public async Task<CartViewModel> AddProductToGuestCartAsync(Guid productId)
        {
            
			string sessionId = _httpContextAccessor.HttpContext.Request.Cookies["ShoppingCartSessionId"];
			if (sessionId == null)
            {
               sessionId =  await CreateShoppingCartSession();
            }

            var cart = _context.ShoppingCarts.Include(sci => sci.ShoppingCartItems).ThenInclude(p => p.Product).ThenInclude(ph=>ph.FrontPhoto).FirstOrDefault(c => c.SessionId == Guid.Parse(sessionId));
             
            if (cart == null)
            {
				cart = new ShoppingCart
				{
					SessionId = Guid.Parse(sessionId)

				};
               
                _context.ShoppingCarts.Add(cart);
            }
            if (cart.ShoppingCartItems.Any(p=>p.Product.Id == productId))
            {
                cart.ShoppingCartItems.FirstOrDefault(p => p.Product.Id == productId).Quantity += 1;
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.ShoppingCartItems.AddAsync(new ShoppingCartItem
                {
                    ProductId = productId,
                    SessionId = Guid.Parse(sessionId),
                    CartId = cart.Id,
                    Quantity = 1
                });
               
                await _context.SaveChangesAsync();
                cart = _context.ShoppingCarts.Include(sci => sci.ShoppingCartItems).ThenInclude(p => p.Product).ThenInclude(ph=>ph.FrontPhoto).FirstOrDefault(c => c.SessionId == Guid.Parse(sessionId));
            }
           

            return new CartViewModel
            {
                Id = cart.Id,
               SessionId = Guid.Parse(sessionId),
                ShoppingCartItems = cart.ShoppingCartItems.Select(sci => new ShoppingCartItemViewModel
                {
                    Id = sci.Id,
                    ProductId = sci.ProductId,
                    CartId = sci.CartId,
                    Quantity = sci.Quantity,
                    Product = new CartProductViewModel
                    {
                        Id = sci.Product!.Id,
                        Name = sci.Product.Name,
                        Description = sci.Product.Description,
                        Price = sci.Product.Price,
                        Image = sci.Product.FrontPhoto.Picture,
                    }
                }).ToList()
            };
            
        }
        public async Task<CartViewModel> AddProductToCartAsync(Guid productId, string userId)
        {
            
            var cart = _context.ShoppingCarts.Include(sci => sci.ShoppingCartItems).ThenInclude(p => p.Product).ThenInclude(ph=>ph.FrontPhoto).FirstOrDefault(c => c.UserId == Guid.Parse(userId));

            

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = Guid.Parse(userId)

                };

                _context.ShoppingCarts.Add(cart);

            }
            if (cart.ShoppingCartItems.Any(p => p.Product.Id == productId))
            {
                cart.ShoppingCartItems.FirstOrDefault(p => p.Product.Id == productId).Quantity += 1;
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.ShoppingCartItems.AddAsync(new ShoppingCartItem
                {
                    ProductId = productId,
                    UserId = Guid.Parse(userId),
                    CartId = cart.Id,
                    Quantity = 1
                });

                await _context.SaveChangesAsync();
                cart = _context.ShoppingCarts.Include(sci => sci.ShoppingCartItems).ThenInclude(p => p.Product).ThenInclude(ph => ph.FrontPhoto).FirstOrDefault(c => c.UserId == Guid.Parse(userId));
            }

           
            return new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                ShoppingCartItems = cart.ShoppingCartItems.Select(sci => new ShoppingCartItemViewModel
                {
                    Id = sci.Id,
                    ProductId = sci.ProductId,
                    CartId = sci.CartId,
                    Quantity = sci.Quantity,
                    Product = new CartProductViewModel
                    {
                        Id = sci.Product!.Id,
                        Name = sci.Product.Name,
                        Description = sci.Product.Description,
                        Price = sci.Product.Price,
                        Image = sci.Product.FrontPhoto.Picture,
                    }
                }).ToList()
            };
        }

        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts.Include(sci=>sci.ShoppingCartItems)
                .ThenInclude(p=>p.Product).ThenInclude(ph=>ph.FrontPhoto)

                .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));

            if (cart == null)
            {
                
                return new CartViewModel();

            }

            return new CartViewModel
            {
                Id = cart.Id,
                UserId = cart.UserId,
                ShoppingCartItems = cart.ShoppingCartItems.Select(sci => new ShoppingCartItemViewModel
                {
                    Id = sci.Id,
                    ProductId = sci.ProductId,
                    CartId = sci.CartId,
                    Quantity = sci.Quantity,
                    Product = new CartProductViewModel
                    {
                        Id = sci.Product!.Id,
                        Name = sci.Product.Name,
                        Description = sci.Product.Description,
                        Price = sci.Product.Price,
                        Image = sci.Product.FrontPhoto.Picture,
                       
                    }
                }).ToList()

            };

            
        }
        public async Task<CartViewModel> GetGuestCartAsync(string sessionId)
        {
            string curSessionId = sessionId;
            var cart = await _context.ShoppingCarts.Include(sci => sci.ShoppingCartItems)
                .ThenInclude(p => p.Product).ThenInclude(ph=>ph.FrontPhoto)

                .FirstOrDefaultAsync(c => c.SessionId == Guid.Parse(sessionId));

            if (cart == null)
            {
                if(sessionId!=null)
                {
                    cart = new ShoppingCart
                    {
                        SessionId = Guid.Parse(sessionId)

                    };
                    _context.ShoppingCarts.Add(cart);
                    await _context.SaveChangesAsync();
                    return new CartViewModel
                    {
                        Id = cart.Id,
                        SessionId = cart.SessionId,
                        ShoppingCartItems = cart.ShoppingCartItems.Select(sci => new ShoppingCartItemViewModel
                        {
                            Id = sci.Id,
                            ProductId = sci.ProductId,
                            CartId = sci.CartId,
                            Quantity = sci.Quantity,
                            Product = new CartProductViewModel
                            {
                                Id = sci.Product!.Id,
                                Name = sci.Product.Name,
                                Description = sci.Product.Description,
                                Price = sci.Product.Price,
                                Image = sci.Product.FrontPhoto.Picture,

                            }
                        }).ToList()
                    };
                }
                return new CartViewModel();
            }

            return new CartViewModel
            {
                Id = cart.Id,
                SessionId = cart.SessionId,
                ShoppingCartItems = cart.ShoppingCartItems.Select(sci => new ShoppingCartItemViewModel
                {
                    Id = sci.Id,
                    ProductId = sci.ProductId,
                    CartId = sci.CartId,
                    Quantity = sci.Quantity,
                    Product = new CartProductViewModel
                    {
                        Id = sci.Product!.Id,
                        Name = sci.Product.Name,
                        Description = sci.Product.Description,
                        Price = sci.Product.Price,
                        Image = sci.Product.FrontPhoto.Picture,

                    }
                }).ToList()

            };


        }

        public async  Task<IEnumerable<CartProductViewModel>> GetGuestCartProductsAsync(Guid sessionId)
        {
           var prducts = await _context.ShoppingCartItems.Include(p=>p.Product).Where(c => c.SessionId == sessionId)
                .Select(sci => new CartProductViewModel
                {
                    Id = sci.Product!.Id,
                    Name = sci.Product.Name,
                    Description = sci.Product.Description,
                    Price = sci.Product.Price,
                }).ToListAsync();

            return prducts;
            
           
        }

        public Task<int> GetCartProductsCountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveProduct(Guid productId, string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));
            string sessionId = _httpContextAccessor.HttpContext.Request.Cookies["ShoppingCartSessionId"];
            ShoppingCartItem? cartItem = null;
            if (cart == null)
            {
                return;
            }
            if (userId == null)
            {
                 cartItem = cart.ShoppingCartItems.FirstOrDefault(sci => sci.SessionId == Guid.Parse(sessionId));
            }
            else
            {
                cartItem = cart.ShoppingCartItems.FirstOrDefault(sci => sci.UserId == Guid.Parse(userId) && sci.ProductId == productId);
            }

            

            if (cartItem == null)
            {
                return;
            }

            if (cartItem.Quantity >= 1)
            {
                cartItem.Quantity--;
                if (cartItem.Quantity == 0)
                {
                    _context.ShoppingCartItems.Remove(cartItem);
                }
            }
            else
            {
                _context.ShoppingCartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();


        }
        public async Task RemoveGuestProduct(Guid productId)
        {
            string sessionId = _httpContextAccessor.HttpContext.Request.Cookies["ShoppingCartSessionId"];
            var cart = await _context.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .FirstOrDefaultAsync(c => c.SessionId == Guid.Parse(sessionId));
            
            ShoppingCartItem? cartItem = null;
            
            cartItem = cart.ShoppingCartItems.FirstOrDefault(sci => sci.SessionId == Guid.Parse(sessionId) && sci.ProductId == productId);
            
            


            if (cartItem == null)
            {
                return;
            }

            if (cartItem.Quantity >= 1)
            {
                cartItem.Quantity--;
                if (cartItem.Quantity == 0)
                {
                    _context.ShoppingCartItems.Remove(cartItem);
                }
            }
            else
            {
                _context.ShoppingCartItems.Remove(cartItem);
            }

            await _context.SaveChangesAsync();


        }

        public Task<decimal> GetCartTotalPriceAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllProductsFromCartAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProductFromCartAsync(Guid productId, Guid userId)
        {
            throw new NotImplementedException();
        }

		public async Task<string> CreateShoppingCartSession()
		{
			string sessionId = Guid.NewGuid().ToString();

			// Save the session identifier to a cookie
			_httpContextAccessor.HttpContext.Response.Cookies.Append("ShoppingCartSessionId", sessionId);

			// Create a new shopping cart session in the database
			var shoppingCartSession = new ShoppingCartSession
			{
				SessionId = Guid.Parse(sessionId)
				
				// Add other properties as needed
			};

			_context.ShoppingCartSessions.Add(shoppingCartSession);
			await _context.SaveChangesAsync();

			return sessionId;
		}

		public async Task<List<ShoppingCartItemViewModel>> GetCartItems()
		{
			// Retrieve the session identifier from the cookie
			string sessionId = _httpContextAccessor.HttpContext.Request.Cookies["ShoppingCartSessionId"];

			if (sessionId != null)
			{
				// Retrieve cart items associated with the session identifier
				var items =  await _context.ShoppingCartItems.Include(p=>p.Product).Where(c => c.SessionId == Guid.Parse(sessionId))
                    .Select(sci=> new ShoppingCartItemViewModel 
                    {
                        Id = sci.Id,
                        SessionId = sci.SessionId,
						ProductId = sci.ProductId,
						CartId = sci.CartId,
						Quantity = sci.Quantity,
						Product = new CartProductViewModel
                        {
							Id = sci.Product!.Id,
							Name = sci.Product.Name,
							Description = sci.Product.Description,
							Price = sci.Product.Price,
						}
					}).ToListAsync();
                     
                return items;

                
			}
            
			return new List<ShoppingCartItemViewModel>(); // Return an empty list if session identifier is not found
		}

       
    }
}
