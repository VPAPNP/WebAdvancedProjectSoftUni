using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{

    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<CartViewModel> AddProductToCartAsync(Guid productId, string userId)
        {
            
            var cart = await _context.ShoppingCarts
                .Include(sci=>sci.ShoppingCartItems)
                .ThenInclude(p=>p.Product)
                .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = Guid.Parse(userId)
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
                    CartId = cart.Id,
                    Quantity = 1
                });
               
                await _context.SaveChangesAsync();
                cart = _context.ShoppingCarts.Include(sci => sci.ShoppingCartItems).ThenInclude(p => p.Product).FirstOrDefault(c => c.UserId == Guid.Parse(userId));
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
                    Product = new ProductViewModel
                    {
                        Id = sci.Product!.Id,
                        Name = sci.Product.Name,
                        Description = sci.Product.Description,
                        Price = sci.Product.Price,
                    }
                }).ToList()
            };
            
        }

        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts.Include(sci=>sci.ShoppingCartItems)
                .ThenInclude(p=>p.Product)

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
                    Product = new ProductViewModel
                    {
                        Id = sci.Product!.Id,
                        Name = sci.Product.Name,
                        Description = sci.Product.Description,
                        Price = sci.Product.Price,
                       
                    }
                }).ToList()

            };

            
        }

        public  Task<IEnumerable<ProductViewModel>> GetCartProductsAsync(Guid userId)
        {
           throw new NotImplementedException();
            
           
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

            if (cart == null)
            {
                return;
            }

            var cartItem = cart.ShoppingCartItems.FirstOrDefault(sci => sci.Id == productId);

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
    }
}
