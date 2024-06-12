using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopWebApp.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property("IsDeleted")
                .HasDefaultValue(false);
            

            builder.HasMany(c => c.Products)
                .WithOne(pc => pc.MainCategory)
                .HasForeignKey(pc => pc.MainCategoryId);

        }
    }
}
