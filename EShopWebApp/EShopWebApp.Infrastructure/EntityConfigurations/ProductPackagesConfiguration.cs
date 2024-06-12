using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopWebApp.Infrastructure.EntityConfigurations
{
    public class ProductPackagesConfiguration : IEntityTypeConfiguration<ProductPackages>
    {
        public void Configure(EntityTypeBuilder<ProductPackages> builder)
        {
            builder.HasKey(pp => new { pp.ProductId, pp.PackageId });

            builder.HasOne(pp => pp.Product)
                .WithMany(p => p.ProductsPackages)
                .HasForeignKey(pp => pp.ProductId);

            builder.HasOne(pp => pp.Package)
                .WithMany(p => p.ProductsPackages)
                .HasForeignKey(pp => pp.PackageId);
        }
    }
}
