using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopWebApp.Infrastructure.Data.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
           
            builder.Property(d => d.CreatedOn)
                 .HasDefaultValueSql("GETDATE()");
            builder.Property(o => o.TotalPrice)
                .HasPrecision(18, 2);
        }
    }
}
