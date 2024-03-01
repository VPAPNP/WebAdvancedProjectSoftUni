using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EShopWebApp.Infrastructure.DataConstants.EntityValidationConstants.Product;
namespace EShopWebApp.Infrastructure.Data.Models
{
    /// <summary>
    /// This class represents the product table.
    /// </summary>
    [Comment("Product table")]
    public class Product
    {
        /// <summary>
        /// The unique identifier for the product.
        /// </summary>
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        [MaxLength(NameMaxLength)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(DescriptionMaxLength)]
        public required string Description { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public  Guid PhotoId { get; set; }
        [ForeignKey(nameof(PhotoId))]
        public Photo Photo { get; set; } = null!;
        [Required]
        public required Guid CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
        public bool IsDeleted { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public Guid BrandId { get; set; }
        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; } = null!;
        













    }

    
}
