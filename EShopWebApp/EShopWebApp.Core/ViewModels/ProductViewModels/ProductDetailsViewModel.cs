using EShopWebApp.Core.ViewModels.CartViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.PhotoViewModels;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductAllViewModel Product { get; set; } 

        public List<ProductAllViewModel> RelatedProducts { get; set; } = new List<ProductAllViewModel>();

        public List<PhotoViewModel> Photos { get; set; } = new List<PhotoViewModel>();

        public List<CategoryViewModel> ProductCategories { get; set; } = new List<CategoryViewModel>();

        public ShoppingCartItemViewModel ShoppingCartItem { get; set; } = new ShoppingCartItemViewModel();
    }
}
