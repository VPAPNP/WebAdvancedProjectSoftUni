using System.ComponentModel.DataAnnotations.Schema;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Table("ProductBrands")]
    public class ProductBrand
    {
        
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        public Guid BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; } = null!;
        
    }
}
