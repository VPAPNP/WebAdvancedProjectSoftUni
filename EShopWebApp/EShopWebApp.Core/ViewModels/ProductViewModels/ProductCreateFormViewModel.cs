using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.PackageViewModels;
using EShopWebApp.Core.ViewModels.PhotoViewModels;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;


namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductCreateFormViewModel
    {
       
        public string Name { get; set; } = null!;
        [Display(Name = "Short Description")]
        public string Description { get; set; } = null!;
       
        public decimal Price { get; set; }
        [Display(Name = "Detailed Description")]
        public string LongDescription { get; set; } = null!;
        
       
        public decimal? DiscountPrice { get; set; }
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }
        [Display(Name = "Brand")]
        public string? BrandId { get; set; }
        [Display(Name = "Package")]
        public string? PackageId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
        [Display(Name = "Main Category")]
        public string CategoryId { get; set; } = null!;

        public CategoryCreateViewModel Category { get; set; } = new CategoryCreateViewModel();

        public PackageViewModel? Package { get; set; } = new PackageViewModel();
       
        [Display(Name = "Categories")]
        public List<Guid> SelectedCategoryIds { get; set; } = new List<Guid>();

        public ICollection<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
        public ICollection<BrandViewModel> Brands { get; set; } = new HashSet<BrandViewModel>();

        public ICollection<PackageViewModel> Packages { get; set; } = new HashSet<PackageViewModel>();
    }
}
