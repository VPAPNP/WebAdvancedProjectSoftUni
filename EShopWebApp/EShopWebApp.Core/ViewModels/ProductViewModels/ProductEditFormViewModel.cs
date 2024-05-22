using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.PhotoViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Product;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductEditFormViewModel
    {
        public string? Id { get; set; }
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;
        [Required]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }
        
        public Guid MainPhotoId { get; set; }

        public string PhotoName { get; set; } = null!;

        public byte[]? MainPhoto { get; set; } = null!;
        [Required]
        [MaxLength(LongDescriptionMaxLength)]
        public string LongDescription { get; set; } = null!;
        [Required]
        
        public int StockQuantity { get; set; }
        [DisplayName("Select Brand")]
        [Required]
        public Guid BrandId { get; set; } 
        public CategoryCreateViewModel Category { get; set; } = new CategoryCreateViewModel();
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [DisplayName("Select Category")]
        [Required]
        public Guid CategoryId { get; set; } 
        public ICollection<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
        public ICollection<BrandViewModel> Brands { get; set; } = new HashSet<BrandViewModel>();
        [Display(Name = "Categories")]
        public List<Guid> SelectedCategoryIds { get; set; } = new List<Guid>();
        public ICollection<PhotoViewModel> Images { get; set; } = new HashSet<PhotoViewModel>();
    }
}
