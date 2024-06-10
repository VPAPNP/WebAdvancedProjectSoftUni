using EShopWebApp.Infrastructure.Data.Models;

namespace EShopWebApp.Core.ViewModels.PackageViewModels
{
    public class PackageViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Weight { get; set; }
        
        public Guid? ProductId { get; set; }

        public Product? Product { get; set; } 

    }
}
