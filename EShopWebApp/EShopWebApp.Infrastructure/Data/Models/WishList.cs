using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("WishList table")]
    public class WishList
    {
        public Guid WishListId { get; set; } = Guid.NewGuid();

        // Foreign key
        public Guid UserId { get; set; }

        // Navigation property
        [ForeignKey(nameof(UserId))]
        public ApplicationUser Customer { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = null!;
    }
}
