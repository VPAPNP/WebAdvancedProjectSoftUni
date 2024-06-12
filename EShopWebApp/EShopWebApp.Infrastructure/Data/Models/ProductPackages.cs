using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShopWebApp.Infrastructure.Data.Models
{
    public class ProductPackages
    {
        
        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        [ForeignKey(nameof(PackageId))]
        public Guid PackageId { get; set; }
        public Package Package { get; set; } = null!;
    }
}
