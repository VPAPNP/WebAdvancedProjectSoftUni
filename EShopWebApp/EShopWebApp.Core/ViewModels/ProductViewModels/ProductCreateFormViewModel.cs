using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.ImageViewModels;


namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductCreateFormViewModel
    {
       
        public string Name { get; set; } = null!;
       
        public string Description { get; set; } = null!;
       
        public decimal Price { get; set; }
       
        

        public decimal? DiscountPrice { get; set; }

        public int StockQuantity { get; set; }

        public string? BrandId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CategoryId { get; set; } = null!;

        public ICollection<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
        public ICollection<BrandViewModel> Brands { get; set; } = new HashSet<BrandViewModel>();
    }
}
