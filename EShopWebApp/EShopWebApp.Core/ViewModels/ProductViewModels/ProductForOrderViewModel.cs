using EShopWebApp.Core.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductForOrderViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal? DiscountPrice { get; set; }
        public CategoryViewModel Category { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string ProductBrand { get; set; } = null!;
    }
}
