using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShopWebApp.Core.ViewModels.ImageViewModels;

namespace EShopWebApp.Core.ViewModels.ProductViewModels
{
    public class ProductEditFormViewModel
    {
        public string Name { get; set; } = null!;


        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public Guid ImageId { get; set; }

        public string ImageName { get; set; } = null!;

        

        public int StockQuantity { get; set; }
        [DisplayName("Select Brand")]
        public string BrandId { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        [DisplayName("Select Category")]
        public string CategoryId { get; set; } = null!;
        public ICollection<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
        public ICollection<BrandViewModel> Brands { get; set; } = new HashSet<BrandViewModel>();
    }
}
