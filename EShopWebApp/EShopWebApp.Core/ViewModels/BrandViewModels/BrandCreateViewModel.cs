using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Brand;
namespace EShopWebApp.Core.ViewModels.BrandViewModels
{
    public class BrandCreateViewModel
    {
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
