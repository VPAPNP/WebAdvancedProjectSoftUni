using EShopWebApp.Core.ViewModels.ProductViewModels;

namespace EShopWebApp.Core.Services.ServiceModels
{
    public class AllProductsFilteredAndPagedServiceModel
    {
        public int TotalProducts { get; set; }
        public IEnumerable<AllProductViewForSearch> Products { get; set; } = new HashSet<AllProductViewForSearch>();
    }
}
