using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Brand;

namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("Brands of products")]
    public class Brand
    {
        [Comment("Brand id")]
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Comment("Brand name")]
        [Required]
        [MaxLength(NameMaxLength)]
		
		public string Name { get; set; } = null!;
        public bool IsDeleted { get; set; }
        
    }
}
