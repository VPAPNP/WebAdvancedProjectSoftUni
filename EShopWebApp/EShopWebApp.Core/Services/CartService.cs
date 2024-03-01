using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Infrastructure;
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

                .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));

            if (cart == null)
            {
                cart = new ShoppingCart
                {
                    UserId = Guid.Parse(userId)
                };
                _context.ShoppingCarts.Add(cart);
            }
            var product = await _context.Products.FindAsync(productId);

            return new CartViewModel();

            
            
            

           

           

            

            await _context.SaveChangesAsync();

           
            
            
            
        }

        public async Task<CartViewModel> GetCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts

                .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));

            //if (cart == null)
            //{
            //    return new CartViewModel();
            //}

            //return new CartViewModel
            //{
            //    Id = cart.Id,
            //    UserId = cart.UserId,
            //    ShoppingCartItems = cart.ShoppingCartItems.Select(sci => new ShoppingCartItemViewModel
            //    {
            //        Id = sci.Id,
            //        ProductId = sci.ProductId,
            //        CartId = sci.CartId,
            //        Quantity = sci.Quantity,
            //        Product = new ProductViewModel
            //        {
            //            Id = sci.Product.Id,
            //            Name = sci.Product.Name,
            //            Description = sci.Product.Description,
            //            Price = sci.Product.Price,
            //            ImageUrl = sci.Product.ImageUrl
            //        }
            //    }).ToList()

            //};

            return new CartViewModel();
        }

        public  Task<IEnumerable<T>> GetCartProductsAsync<T>(Guid userId)
        {
           throw new NotImplementedException();
            
           
        }

        public Task<int> GetCartProductsCountAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveProduct(Guid productId, string userId)
        {
            //var cart = await _context.Carts
            //    .Include(c => c.ShoppingCartItems)
            //    .FirstOrDefaultAsync(c => c.UserId == Guid.Parse(userId));

            //if (cart == null)
            //{
            //    return;
            //}

            //var cartItem = cart.ShoppingCartItems.FirstOrDefault(sci => sci.Id == productId);

            //if (cartItem == null)
            //{
            //    return;
            //}

            //if (cartItem.Quantity >= 1)
            //{
            //    cartItem.Quantity--;
            //}
            //else
            //{
            //    _context.ShoppingCartItems.Remove(cartItem);
            //}

            //await _context.SaveChangesAsync();
            

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
