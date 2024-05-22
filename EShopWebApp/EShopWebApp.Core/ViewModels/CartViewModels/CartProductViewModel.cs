using EShopWebApp.Infrastructure.Data.Enums;

namespace EShopWebApp.Core.ViewModels.CartViewModels
{
    public class CartProductViewModel
    {
        public Guid Id { get; set; } = new Guid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public byte[] Image { get; set; } = null!;
        public string CategoryName { get; set; } = string.Empty;

        public PackageType PackageType { get; set; }

        public int Quantity { get; set; }
    }
}