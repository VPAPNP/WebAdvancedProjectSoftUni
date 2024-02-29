using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Table("ProductsPhotos")]
    public class ProductPhoto
    {
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }    = null!;
        public Guid PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public Photo Photo { get; set; } = null!;

    }
}
