using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopWebApp.Infrastructure.EntityConfigurations
{
    public class PackageConfigurations : IEntityTypeConfiguration<Package>
    {
        public void Configure(EntityTypeBuilder<Package> builder)
        {
            builder.Property(p => p.Weight)
                .HasPrecision(18, 2);

        }
    }
}
