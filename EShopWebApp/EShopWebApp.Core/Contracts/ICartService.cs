using EShopWebApp.Core.ViewModels.CartViewModels;

namespace EShopWebApp.Core.Contracts
{
    public interface ICartService
    {
        Task<CartViewModel> AddProductToCartAsync(Guid productId, string userId);
        Task RemoveProductFromCartAsync(Guid productId, Guid userId);
        Task RemoveAllProductsFromCartAsync(Guid userId);
        Task<IEnumerable<T>> GetCartProductsAsync<T>(Guid userId);
        Task<decimal> GetCartTotalPriceAsync(Guid userId);
        Task<int> GetCartProductsCountAsync(Guid userId);
        Task<CartViewModel> GetCartAsync(string userId);

        Task RemoveProduct(Guid productId, string userId);
    }
}
