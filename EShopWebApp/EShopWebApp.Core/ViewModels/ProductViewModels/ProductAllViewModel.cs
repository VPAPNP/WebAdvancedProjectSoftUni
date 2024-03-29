﻿using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.ImageViewModels;
using EShopWebApp.Infrastructure.Data.Models;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductAllViewModel
    {
        public required string Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int StockQuantity { get; set; }
        public decimal? DiscountPrice { get; set; }
        public CategoryViewModel Category { get; set; } = null!;
        
        public byte[] Image { get; set; } = null!;
        
        public decimal Price { get; set; }
        public string ProductBrand { get; set; } = null!;
        
    }

    
}
