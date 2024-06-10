using EShopWebApp.Infrastructure.Data.Enums;

namespace EShopWebApp.Infrastructure.Data.Models
{
    public class Package
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public PackageType PackageType { get; set; }
		public decimal Weight { get; set; }
		public bool IsDeleted { get; set; }
		

		public ICollection<ProductPackages> ProductsPackages { get; set; } = new HashSet<ProductPackages>();
		
	}
}
