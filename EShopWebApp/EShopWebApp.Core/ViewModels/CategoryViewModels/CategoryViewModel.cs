using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Category;

namespace EShopWebApp.Core.ViewModels.CategoryViewModels
{
    public class CategoryViewModel
    {
        public required string Id { get; set; }
        [Comment("This is a category name")]
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
