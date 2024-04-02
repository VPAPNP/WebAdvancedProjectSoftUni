using EShopWebApp.Core.ViewModels.CategoryViewModels;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class AllProductViewForSearch
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal? DiscountPrice { get; set; }
        public CategoryViewModel Category { get; set; } = null!;
        public byte[] Image { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string ProductBrand { get; set; } = null!;
    }
}
