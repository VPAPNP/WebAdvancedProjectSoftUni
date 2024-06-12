using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopWebApp.Infrastructure.Data.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(d=>d.CreatedOn)
                .HasDefaultValueSql("GETDATE()");
            builder.Property(p => p.Price)
                .HasPrecision(18, 2);
            
            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.HasMany(p => p.ProductCategories)
                .WithOne(pc => pc.Product)
                .HasForeignKey(pc => pc.ProductId);
           
           


        }
    }
}
