using EShopWebApp.Core.ViewModels.CartViewModels;

namespace EShopWebApp.Core.Contracts
{
    public interface ICartService
    {
        
        Task<CartViewModel> AddProductToGuestCartAsync(Guid productId);
        Task<CartViewModel> AddProductToCartAsync(Guid productId, string userId);
        Task RemoveProductFromCartAsync(Guid productId, Guid userId);
        Task RemoveAllProductsFromCartAsync(Guid userId);
        Task<decimal> GetCartTotalPriceAsync(Guid userId);
        Task<int> GetCartProductsCountAsync(Guid userId);
        Task<CartViewModel> GetCartAsync(string userId);
        Task<CartViewModel> GetGuestCartAsync(string sessionId);
        Task<IEnumerable<CartProductViewModel>> GetGuestCartProductsAsync(Guid sessionId);
        Task RemoveProduct(Guid productId, string userId);
        Task RemoveGuestProduct(Guid productId);
        Task<string> CreateShoppingCartSession();
        Task<List<ShoppingCartItemViewModel>> GetCartItems();

	}
}
