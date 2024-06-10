using EShopWebApp.Core.ViewModels.PackageViewModels;
using EShopWebApp.Infrastructure.Data.Models;

namespace EShopWebApp.Core.Contracts
{
    public interface IPackageService
    {
        Task<ICollection<PackageViewModel>> GetAllAsync();
        Task<PackageViewModel> GetByIdAsync(Guid id);
        Task CreateAsync(PackageViewModel package);
        Task UpdateAsync(PackageViewModel package);
        Task DeleteAsync(Guid id);


    }
}
