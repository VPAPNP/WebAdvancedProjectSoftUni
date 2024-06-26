﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Category;
namespace EShopWebApp.Infrastructure.Data.Models
{
    [Comment("This is a category table")]
    public class Category
    {

        public Category()
        {
            Id = Guid.NewGuid();
        }
        [Comment("This is a category id")]
        [Key]
        public Guid Id { get; set; }
        [Comment("This is a category name")]
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;
        public Guid? ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
		
		public bool IsDeleted { get; set; }
        public ICollection<Product> Products { get; set; }



    }
}
