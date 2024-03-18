using EShopWebApp.Core.ViewModels.CategoryViewModels;

namespace EShopWebApp.Core.Contracts
{
    public interface ICategoryService
    {
        Task UndoDeleteAsync(string id);
        Task <ICollection<CategoryViewModel>> GetAllAsync();

        Task<CategoryViewModel> GetByIdAsync(Guid id);

        Task<CategoryViewModel> GetByNameAsync(string name);

        Task CreateAsync(CategoryCreateViewModel categoryCreateViewModel);

        Task DeleteAsync(string id);
        Task<bool> ExistsByNameAsync(string name);

        Task <List<string>> GetAllNamesAsync();

        Task EditAsync(CategoryViewModel categoryEditViewModel);
    }
}
