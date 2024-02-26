using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Image;
namespace EShopWebApp.Infrastructure.Data.Models
{
    public class Image
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(ImageMaxLength)]
        public required string Name { get; set; } 
        public byte[] Picture { get; set; } = null!;
       
    }
}
