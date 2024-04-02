using System.ComponentModel.DataAnnotations;
using EShopWebApp.Core.ViewModels.ImageViewModels;
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
        [Required]
        [Range(PriceMinValue, PriceMaxValue)]
        public decimal Price { get; set; }
        public string? PhotoName { get; set; } 
        public Guid ImageId { get; set; }
        [Required]
        [Range(QuantityMinValue, QuantityMaxValue)]
        public int StockQuantity { get; set; }
        public string BrandId { get; set; } = null!;
        public DateTime? ModifiedOn { get; set; }
        public string CategoryId { get; set; } = null!;
    }
}
