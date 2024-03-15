using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.PaymentInfo;
namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("PaymentInfo table")]
    public class PaymentInfo
    {
        [Key]
        public Guid PaymentId { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(CardNumberMaxLength)]
        public required string CardNumber { get; set; }
        public DateTime ExpirationDate { get; set; }

        // Foreign key
        public Guid OrderId { get; set; }

        // Navigation property
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }
}
