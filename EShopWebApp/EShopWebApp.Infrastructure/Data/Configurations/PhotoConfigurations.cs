using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Infrastructure.Data.Configurations
{
    public class PhotoConfigurations : IEntityTypeConfiguration<Photo>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Photo> builder)
        {
            builder.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(p => p.Product)
                .WithMany(p => p.ProductPhotos)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.NoAction);


           
        }
    }
}
