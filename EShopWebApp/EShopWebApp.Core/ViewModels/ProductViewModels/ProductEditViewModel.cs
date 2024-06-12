using System.ComponentModel.DataAnnotations;
using EShopWebApp.Core.ViewModels.PhotoViewModels;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Product;
namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductEditViewModel
    {
        public string Id { get; set; } = null!;
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;
        [Required]
        [MaxLength(DescriptionMaxLength)]
        [MinLength(DescriptionMinLength)]
        public string Description { get; set; } = null!;
        public string LongDescription { get; set; } = null!;
        [Required]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }
        public string? PhotoName { get; set; } 
        public Guid MainPhotoId { get; set; }
        [Required]
        [Range(QuantityMinValue, QuantityMaxValue)]
        public int StockQuantity { get; set; }
        [Required]
        public Guid BrandId { get; set; }
        public byte[]? MainPhoto { get; set; } = null!;
        public DateTime? ModifiedOn { get; set; }
        [Required]
        public Guid CategoryId { get; set; } 

        public IEnumerable<PhotoViewModel> Images { get; set; } = new List<PhotoViewModel>();

        public List<Guid> SelectedCategoryIds { get; set; } = new List<Guid>();
    }
}
