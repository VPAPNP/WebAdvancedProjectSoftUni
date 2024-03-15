using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("OrderItem table")]
    public class OrderItem
    {
        public Guid OrderItemId { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; }

        // Foreign keys
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
