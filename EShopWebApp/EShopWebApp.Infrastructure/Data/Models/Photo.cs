using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Image;
namespace EShopWebApp.Infrastructure.Data.Models
{
    
    public class Photo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(ImageMaxLength)]
        public required string Name { get; set; } 
        public byte[] Picture { get; set; } = null!;
        public bool IsDeleted { get; set; }

       
    }
}
