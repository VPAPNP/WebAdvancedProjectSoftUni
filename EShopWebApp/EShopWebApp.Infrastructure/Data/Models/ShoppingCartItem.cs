using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("This is a shopping cart item table")]
    public class ShoppingCartItem
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SessionId { get; set; }
        public Guid? UserId { get; set; }
        public Guid CartId { get; set; }
        [ForeignKey(nameof(CartId))]
        public  ShoppingCart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }



    }
}