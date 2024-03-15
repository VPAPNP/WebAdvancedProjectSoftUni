using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.ShippingInfo;
namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("This is a ShippingInfo table")]
    public class ShippingInfo
    {
        [Key]
        public Guid ShippingId { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(RecipientNameMaxLength)]
        public required string RecipientName { get; set; }
        [Required]
        [MaxLength(AddressMaxLength)]
        public required string Address { get; set; }
        [Required]
        [MaxLength(CityMaxLength)]
        public required string PhoneNumber { get; set; }

        // Foreign key
        public Guid OrderId { get; set; }

        // Navigation property
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }
}
