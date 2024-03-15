using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UndoDeleteAsync(string id)
        {
            var category = await _dbContext.Categories.FirstAsync(c => c.Id == Guid.Parse(id));
            category.IsDeleted = false;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ICollection<CategoryViewModel>> GetAllAsync()
        {
            
            var categories = await _dbContext.Categories.Where(c=>c.IsDeleted == false).Select(c => new CategoryViewModel()
            {
                Id = c.Id.ToString(),
                Name = c.Name
            }).ToListAsync();

            return categories;
        }

        public async Task<CategoryViewModel> GetByIdAsync(Guid id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            var categoryViewModel = new CategoryViewModel()
            {
                Id = category!.Id.ToString(),
                Name = category.Name
            };
            return categoryViewModel;
        }

        public async Task<CategoryViewModel> GetByNameAsync(string name)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name);
            var categoryViewModel = new CategoryViewModel()
            {
                Id = category!.Id.ToString(),
                Name = category.Name
            };
            return categoryViewModel;
        }

        public async Task CreateAsync(CategoryCreateViewModel categoryCreateViewModel)
        {
            var category = new Category()
            {
                Name = categoryCreateViewModel.Name
            };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<bool> ExistsByNameAsync(string name)
        {
            var category = await _dbContext.Categories.AnyAsync(c => c.Name == name);
            

            return category;
        }


        public async Task DeleteAsync(string id)
        {
            var category = await _dbContext.Categories.FirstAsync(c => c.Id == Guid.Parse(id));
            category.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<string>> GetAllNamesAsync()
        {
            var categories = _dbContext.Categories.Select(c => c.Name).ToListAsync();

            return categories;
        }
    }
}
