using EShopWebApp.Core.ViewModels.BrandViewModels;

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

    }
}
