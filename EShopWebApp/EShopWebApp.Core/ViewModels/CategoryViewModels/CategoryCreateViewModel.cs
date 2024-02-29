using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Category;
namespace EShopWebApp.Core.ViewModels.CategoryViewModels
{
    public class CategoryCreateViewModel
    {
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
