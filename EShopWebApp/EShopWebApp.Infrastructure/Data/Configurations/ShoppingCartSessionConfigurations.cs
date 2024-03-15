using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShopWebApp.Infrastructure.Data.Configurations
{
	public class ShoppingCartSessionConfigurations : IEntityTypeConfiguration<ShoppingCartSession>
	{
		public void Configure(EntityTypeBuilder<ShoppingCartSession> builder)
		{
			builder.HasKey(x => x.SessionId);
			builder.Property(x => x.CreatedAt).HasDefaultValueSql("getdate()");
		}

		
	}
	
	
}
