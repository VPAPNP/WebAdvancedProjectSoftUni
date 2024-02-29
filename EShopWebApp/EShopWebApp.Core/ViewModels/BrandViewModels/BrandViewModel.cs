using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Brand;
namespace EShopWebApp.Core.ViewModels.BrandViewModels
{
    public class BrandViewModel
    {
        public required string Id { get; set; }
        [Comment("This is a category name")]
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
