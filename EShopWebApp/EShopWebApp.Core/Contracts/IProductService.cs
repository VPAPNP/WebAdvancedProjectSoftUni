using EShopWebApp.Core.Services.ServiceModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;

namespace EShopWebApp.Core.Contracts
{
    public interface IProductService
    {
        Task<ICollection<ProductAllViewModel>> GetAllAsync();

        Task<Product> GetByIdAsync(Guid id);

        Task CreateAsync(IFormFile file,ProductCreateViewModel productCreateViewModel);
        Task DeleteAsync(Guid id);

        Task EditAsync(Guid id,ProductEditViewModel productEditViewModel);

        Task<AllProductsFilteredAndPagedServiceModel> GetAllFilteredAndPagedAsync(AllProductsQueryModel productsQueryModel);

    }
}
