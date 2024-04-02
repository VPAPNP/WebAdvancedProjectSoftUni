using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{

    public class BrandService : IBrandService
    {
        private readonly ApplicationDbContext _dbContext;

        public BrandService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task UndoDeleteAsync(string id)
        {
            var brand = await _dbContext.Brands.FirstAsync(c => c.Id == Guid.Parse(id));
            brand.IsDeleted = false;
            await _dbContext.SaveChangesAsync();
        }
        public async Task<ICollection<BrandViewModel>> GetAllAsync()
        {

            var brands = await _dbContext.Brands.Where(c => c.IsDeleted == false).Select(c => new BrandViewModel()
            {
                Id = c.Id.ToString(),
                Name = c.Name
            }).ToListAsync();

            return brands;
        }

        public async Task<BrandViewModel> GetByIdAsync(Guid id)
        {
            var brand = await _dbContext.Brands.FirstOrDefaultAsync(c => c.Id == id);
            var brandViewModel = new BrandViewModel()
            {
                Id = brand!.Id.ToString(),
                Name = brand.Name
            };
            return brandViewModel;
        }

        public async Task<BrandViewModel> GetByNameAsync(string name)
        {
            Brand? brand = await _dbContext.Brands.FirstOrDefaultAsync(c => c.Name == name);
            var brandViewModel = new BrandViewModel()
            {
                Id = brand!.Id.ToString(),
                Name = brand.Name
            };
            return brandViewModel;
        }

        public async Task CreateAsync(BrandCreateViewModel brandCreateViewModel)
        {
            var brand = new Brand()
            {
                Name = brandCreateViewModel.Name
            };
            await _dbContext.Brands.AddAsync(brand);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var brand = await _dbContext.Brands.AnyAsync(c => c.Name == name);


            return brand;
        }


        public async Task DeleteAsync(string id)
        {
            var category = await _dbContext.Brands.FirstAsync(c => c.Id == Guid.Parse(id));
            category.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<string>> GetAllNamesAsync()
        {
            var brands = _dbContext.Brands.Select(b => b.Name).ToListAsync();

            return brands;
           
        }

        public async Task EditAsync(BrandViewModel brandEditViewModel)
        {
           var brand = await _dbContext.Brands.FirstAsync(c => c.Id == Guid.Parse(brandEditViewModel.Id));
            brand.Name = brandEditViewModel.Name;
            await _dbContext.SaveChangesAsync();
        }

    }
}
