using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.PackageViewModels;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{
    public class PackageService : IPackageService
    {
        private readonly ApplicationDbContext _context;

        public PackageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(PackageViewModel package)
        {
            var newPackage = new Package
            {
                Name = package.Name,
                Description = package.Description,
                Weight = package.Weight,
                
            };

            await _context.Packages.AddAsync(newPackage);
            await _context.SaveChangesAsync();

            
        }

        public async Task DeleteAsync(Guid id)
        {
            var package = await _context.Packages.FirstOrDefaultAsync(p => p.Id == id);
            package!.IsDeleted = true;
            _context.Packages.Update(package);
            _context.SaveChanges();


        }

        public async Task<ICollection<PackageViewModel>> GetAllAsync()
        {
            var packages =  _context.Packages.Where(p => p.IsDeleted == false);

            var packageViewModels = await packages.Select(p => new PackageViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Weight = p.Weight,
                
            }).ToListAsync();

            return packageViewModels;


        }

        public async Task<PackageViewModel> GetByIdAsync(Guid id)
        {
            var package = await _context.Packages.Where(p=>p.IsDeleted == false).FirstOrDefaultAsync(p => p.Id == id);

            var packageViewModel = new PackageViewModel
            {
                Id = package!.Id,
                Name = package.Name,
                Description = package.Description,
                Weight = package.Weight,
               
            };
            return packageViewModel;
        }

        public async Task UpdateAsync(PackageViewModel package)
        {
            var updatedPackage = new Package
            {
                Id = package.Id,
                Name = package.Name,
                Description = package.Description,
                Weight = package.Weight,
                
            };

           _context.Packages.Update(updatedPackage);
            await _context.SaveChangesAsync();

            

        }
    }
}
