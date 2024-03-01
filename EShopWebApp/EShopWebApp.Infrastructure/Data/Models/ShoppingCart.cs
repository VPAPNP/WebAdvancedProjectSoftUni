using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("ShoppingCart table")]
    public class ShoppingCart
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        public decimal TotalPrice { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        
    }
}