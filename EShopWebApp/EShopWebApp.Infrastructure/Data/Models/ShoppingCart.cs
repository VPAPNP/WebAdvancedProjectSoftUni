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
        public Guid? SessionId { get; set; }
        [ForeignKey(nameof(SessionId))]
        public ShoppingCartSession Session { get; set; } = null!;
        public Guid? UserId { get; set; }
        
        public ApplicationUser User { get; set; } = null!;

        public decimal TotalPrice { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
        
    }
}