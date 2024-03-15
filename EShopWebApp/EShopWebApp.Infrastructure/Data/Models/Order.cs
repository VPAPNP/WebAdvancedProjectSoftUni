using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("Order table")]
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        // Foreign key
        public Guid UserId { get; set; }
        // Navigation properties
        [ForeignKey(nameof(UserId))]
        public ApplicationUser Customer { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; }  = null!;
        public PaymentInfo PaymentInfo { get; set; } = null!;
        public ShippingInfo ShippingInfo { get; set; } =null!;
        
    }
}
