using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Category;
namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("Category table")]
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(NameMaxLength)]
        public required string Name { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
