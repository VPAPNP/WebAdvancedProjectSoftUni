using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.ApplicationUser;

namespace EShopWebApp.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(FirstNameMaxLength)]
        public required string FirstName { get; set; }
        [MaxLength(MiddleNameMaxLength)]
        public string? MiddleName { get; set; }
        [MaxLength(LastNameMaxLength)]
        public required string LastName { get; set; }
        [MaxLength(AddressMaxLength)]
        public string? Address { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
        public ShoppingCart ShoppingCart { get; set; } = null!;
        public WishList WishList { get; set; } =null!;
    }
}
