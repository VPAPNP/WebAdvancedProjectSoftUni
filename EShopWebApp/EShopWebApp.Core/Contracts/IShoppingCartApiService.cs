using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopWebApp.Core.Contracts
{
    public interface IShoppingCartApiService
    {
        Task<string> CreateShoppingCartSession();
        Task AddItemToCart(string sessionId, ShoppingCartItemViewModel item);
        Task<List<ShoppingCartItem>> GetCartItems(string sessionId);
        Task UpdateCartItem(string sessionId, Guid itemId, int quantity);
        Task RemoveItemFromCart(string sessionId, Guid itemId);
    }
}
