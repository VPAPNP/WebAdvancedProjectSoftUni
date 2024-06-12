using EShopWebApp.Core.ViewModels.CartViewModels;

namespace EShopWebApp.Core.Contracts
{
    public interface ICartService
    {
        
        Task<CartViewModel> AddProductToGuestCartAsync(Guid productId);
        Task<CartViewModel> AddProductToCartAsync(Guid productId, string userId);
       
        Task RemoveAllProductsFromCartAsync();
        Task RemoveShoppingCartItemsAsync(string productId,string userId);
        
        Task<CartViewModel> GetCartAsync(string userId);
        Task<CartViewModel> GetGuestCartAsync(string sessionId);
        Task<IEnumerable<CartProductViewModel>> GetGuestCartProductsAsync(Guid sessionId);
        Task RemoveProduct(Guid productId, string userId);
        Task RemoveGuestProduct(Guid productId);
        Task<string> CreateShoppingCartSession();
        Task<List<ShoppingCartItemViewModel>> GetCartItems();

        Task AddCartItemToUserCart(Guid productId, int quantity, string userId);

        Task SetQuantityToCartItem(Guid productId, int quantity, string userId);

        Task<ShoppingCartItemViewModel> GetCartItem(string id);


        
	}
}
