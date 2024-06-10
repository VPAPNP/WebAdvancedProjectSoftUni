using System.ComponentModel.DataAnnotations;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.PackageViewModels;
using EShopWebApp.Core.ViewModels.PhotoViewModels;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Product;



namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductCreateViewModel
    {


        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;
        [MaxLength(LongDescriptionMaxLength)]
        public string LongDescription { get; set; } = null!;
        [Required]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(QuantityMinValue, QuantityMaxValue)]
        public int StockQuantity { get; set; }
        public string BrandId { get; set; } = null!;
        public string PackageId { get; set; } = null!;
        public PackageViewModel? Package { get; set; } = new PackageViewModel();
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string CategoryId { get; set; } = null!;

        public List<Guid> SelectedCategoryIds { get; set; } = new List<Guid>();

        

      
        

    }
}
