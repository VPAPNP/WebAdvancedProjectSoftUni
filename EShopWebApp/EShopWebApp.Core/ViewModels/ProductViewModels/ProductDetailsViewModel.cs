using EShopWebApp.Core.ViewModels.ImageViewModels;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductAllViewModel Product { get; set; } 

        public List<ProductAllViewModel> RelatedProducts { get; set; } = new List<ProductAllViewModel>();

        public List<PhotoViewModel> Photos { get; set; } = new List<PhotoViewModel>();
    }
}
