﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Core.DataConstants.ValidationConstants.Brand;
namespace EShopWebApp.Core.ViewModels.BrandViewModels
{
    public class BrandViewModel
    {
        public required string Id { get; set; }
        [Required]
        [MaxLength(NameMaxLength)]
        [MinLength(NameMinLength)]
        public string Name { get; set; } = null!;
    }
}
