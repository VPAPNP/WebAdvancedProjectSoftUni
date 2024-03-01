using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services.ServiceModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
using EShopWebApp.Core.ViewModels.ImageViewModels;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using EShopWebApp.Core.ViewModels.ProductViewModels.Enums;
using EShopWebApp.Infrastructure.Data;
using EShopWebApp.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EShopWebApp.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly Contracts.IPhotoService _imageService;

        public ProductService(ApplicationDbContext context, Contracts.IPhotoService imageService)
        {
            _context = context;
            _imageService = imageService;
        }
        public async Task<ICollection<ProductAllViewModel>> GetAllAsync()
        {
            var products = await _context.Products.Include(c=>c.Category).Where(p=>p.IsDeleted == false).Select(p=> new ProductAllViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.Quantity,
                Description = p.Description,
                Image = p.Photo.Picture,
              
                Category = new CategoryViewModel()
                {
                    Id = p.Category.Id.ToString(),
                    Name = p.Category.Name
                }
            }).ToListAsync();
               return products;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            return product!;
            
        }

        public async Task  CreateAsync(IFormFile file,ProductCreateViewModel productCreateViewModel)
        {
            

            var image = _imageService.CreateImage(file, file.FileName);
            var photo = await _context.Photos.AddAsync(new Photo
            {
                Name = image.Name,
                Picture = image.Picture
            });
            
            var id = photo.Entity.Id;
           
            var product = new Product
            {
              
                Name = productCreateViewModel.Name,
                Description = productCreateViewModel.Description,
                Price = productCreateViewModel.Price,
                Quantity = productCreateViewModel.StockQuantity,
                PhotoId = id,
                CategoryId = Guid.Parse(productCreateViewModel.CategoryId),
                BrandId = Guid.Parse(productCreateViewModel.BrandId)
                
            };

            
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            product!.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(IFormFile file,Guid id,ProductEditViewModel editProductModel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == product.PhotoId);
            var newPhoto = _imageService.CreateImage(file, file.FileName);
            if (file != null)
            {
                if (photo.Name != newPhoto.Name)
                {
                    photo!.Name = newPhoto.Name;
                    photo.Picture = newPhoto.Picture;
                }
                
               
            }
            
            product!.Name = editProductModel.Name;
            product.Description = editProductModel.Description;
            product.Price = editProductModel.Price;
            product.PhotoId = photo.Id;
            product.Quantity = editProductModel.StockQuantity;
            product.CategoryId = Guid.Parse(editProductModel.CategoryId);
            product.BrandId = Guid.Parse(editProductModel.BrandId);
            product.ModifiedOn = DateTime.UtcNow;
            product.Photo = photo;


            await _context.SaveChangesAsync();
        }

        public async Task<AllProductsFilteredAndPagedServiceModel> GetAllFilteredAndPagedAsync(AllProductsQueryModel productsQueryModel)
        {
            IQueryable<Product> productsQuery = _context
                .Products
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(productsQueryModel.Category))
            {
                productsQuery = productsQuery.Where(p => p.Category.Name == productsQueryModel.Category);
            }
            if (!string.IsNullOrWhiteSpace(productsQueryModel.Brand))
            {
                productsQuery = productsQuery.Where(p => p.Brand.Name == productsQueryModel.Brand);
            }
            if (!string.IsNullOrWhiteSpace(productsQueryModel.SearchTerm))
            {
                string searchTermWildCard = $"%{productsQueryModel.SearchTerm.ToLower()}%";
                productsQuery = productsQuery.Where(p=> 
                EF.Functions.Like(p.Name, searchTermWildCard) || 
                EF.Functions.Like(p.Category.Name.ToString(), searchTermWildCard) ||
                EF.Functions.Like(p.Brand.Name.ToString(), searchTermWildCard) ||
                EF.Functions.Like(p.Description, searchTermWildCard)
                );
            }
            productsQuery =productsQueryModel.ProductSorting switch
            {
                ProductSorting.Newest => productsQuery.OrderBy(p => p.CreatedOn),
                ProductSorting.Oldest => productsQuery.OrderByDescending(p => p.CreatedOn),
                ProductSorting.PriceDesc => productsQuery.OrderByDescending(p => p.Price),
                ProductSorting.PriceAsc => productsQuery.OrderBy(p => p.Price),
                ProductSorting.NameDesc => productsQuery.OrderByDescending(p => p.Name),
                ProductSorting.NameAsc => productsQuery.OrderBy(p => p.Name),
                _ => productsQuery.OrderByDescending(p => p.CreatedOn)
            };
                
           IEnumerable<AllProductViewForSearch>  productAllView  = await  productsQuery
               .Where(p=>p.IsDeleted == false)
                .Skip((productsQueryModel.CurrentPage - 1) * productsQueryModel.PageSize)
                .Take(productsQueryModel.PageSize)
               
                .Select(p => new AllProductViewForSearch
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                   
                    Description = p.Description,
                    Image = p.Photo.Picture,
                    
                   
                    Category = new CategoryViewModel()
                    {
                        Id = p.Category.Id.ToString(),
                        Name = p.Category.Name
                    }
                }).ToListAsync();
                
            int totalProducts = await productsQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalProducts / productsQueryModel.PageSize);
            var allProductsFilteredAndPagedServiceModel = new AllProductsFilteredAndPagedServiceModel
            {
               TotalProducts = totalProducts,
               Products = productAllView
               
            };

            return allProductsFilteredAndPagedServiceModel;


        }
    }
}
