using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;

namespace EShopWebApp.Core.Contracts
{
    public interface IBrandService
    {
        Task UndoDeleteAsync(string id);
        Task<ICollection<BrandViewModel>> GetAllAsync();

        Task<BrandViewModel> GetByIdAsync(Guid id);

        Task<BrandViewModel> GetByNameAsync(string name);

        Task CreateAsync(BrandCreateViewModel categoryCreateViewModel);

        Task DeleteAsync(string id);
        Task<bool> ExistsByNameAsync(string name);

        Task<List<string>> GetAllNamesAsync();
        Task EditAsync(BrandViewModel brandEditViewModel);
    }
}
