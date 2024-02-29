using System.ComponentModel.DataAnnotations.Schema;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Table("ProductCategories")]
    public class ProductCategory
    {
       
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        public Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}
