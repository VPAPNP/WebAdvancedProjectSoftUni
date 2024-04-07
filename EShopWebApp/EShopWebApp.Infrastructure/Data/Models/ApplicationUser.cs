using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.ApplicationUser;

namespace EShopWebApp.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(FirstNameMaxLength)]
        public  string? FirstName { get; set; } = string.Empty;
        [MaxLength(MiddleNameMaxLength)]
        public string? MiddleName { get; set; } = string.Empty;
        [MaxLength(LastNameMaxLength)]
        public  string? LastName { get; set; }   = string.Empty;
        [MaxLength(AddressMaxLength)]
        public string? Address { get; set; }
        public ICollection<Order> Orders { get; set; } = null!;
        public ShoppingCart ShoppingCart { get; set; } 
        public WishList WishList { get; set; } 
    }
}
