using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.Services.ServiceModels;
using EShopWebApp.Core.ViewModels.BrandViewModels;
using EShopWebApp.Core.ViewModels.CategoryViewModels;
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
        private readonly IPhotoService _photoService;

        public ProductService(ApplicationDbContext context,IPhotoService photoService)
        {
            _context = context;
            _photoService = photoService;
        }
		public async Task<ICollection<ProductAllViewModel>> GetAllAsync()
		{
			var products = await _context.Products.Include(c => c.MainCategory).Where(p => p.IsDeleted == false).Select(p => new ProductAllViewModel
			{
				Id = p.Id.ToString(),
				Name = p.Name,
				Price = p.Price,
				StockQuantity = p.Quantity,
				Description = p.Description,
				Image = p.FrontPhoto.Picture,

				Category = new CategoryViewModel()
				{
					Id = p.MainCategory.Id.ToString(),
					Name = p.MainCategory.Name
				}
			}).ToListAsync();

			return products;
		}

		public async Task<ProductAllViewModel> GetByIdAsync(Guid id)
        {
           
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            
            
				product!.Brand = await _context.Brands.FirstOrDefaultAsync(b => b.Id == product.BrandId);
				product.MainCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == product.MainCategoryId);
				product.FrontPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.Id == product.FrontPhotoId);
               
			
           


            var productAllViewModel = new ProductAllViewModel
            {
                Id = product!.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.Quantity,
                Description = product.Description,
                Image = product.FrontPhoto!.Picture,
                Category = new CategoryViewModel()
                {
                    Id = product.MainCategory.Id.ToString(),
                    Name = product.MainCategory.Name
                },
                Brand = new BrandViewModel()
                {
                    Id = product.Brand!.Id.ToString(),
                    Name = product.Brand.Name
                },
                PhotoId = product.FrontPhotoId.ToString(),
                CategoryId = product.MainCategoryId.ToString(),
                BrandId = product.BrandId.ToString(),
                LongDescription = product.LongDescription,
                ProductCategories = product.ProductCategories.Select(c => new CategoryViewModel()
                {
                    Id = c.Id.ToString(),
                    Name = c.Name
                }).ToList()
               
                
                
                
            };
           
            return productAllViewModel;
            
        }

        public async Task  CreateAsync(IEnumerable<IFormFile> files, IFormFile file,ProductCreateViewModel productCreateViewModel)
        {
            

            var image = _photoService.CreateImage(file, file.FileName);
            var photo = await _context.Photos.AddAsync(new Photo
            {
                Name = image.Name,
                Picture = image.Picture
            });
            
            var id = photo.Entity.Id;
           var productCategories = new List<Category>();
            foreach (var categoryId in productCreateViewModel.SelectedCategoryIds)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                productCategories.Add(category!);
            }
            var product = new Product
            {
              
                Name = productCreateViewModel.Name,
                Description = productCreateViewModel.Description,
                Price = productCreateViewModel.Price,
                Quantity = productCreateViewModel.StockQuantity,
                FrontPhotoId = id,
                MainCategoryId = Guid.Parse(productCreateViewModel.CategoryId),
                BrandId = Guid.Parse(productCreateViewModel.BrandId),
                LongDescription = productCreateViewModel.LongDescription,
                ProductCategories = productCategories 
            };

            
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var frontPhoto = await _context.Photos.FirstOrDefaultAsync(p => p.Id == product.FrontPhotoId);
            frontPhoto!.ProductId = product.Id;
            var productPhotos = new List<Photo>();


            foreach (var formFile in files)
            {
                var imageGallery = _photoService.CreateImage(formFile, formFile.FileName);
                var photoToUpload = new Photo 
                {
                    Name = imageGallery.Name,
                    Picture = imageGallery.Picture,
                    ProductId = product.Id
                    
                };
                productPhotos.Add(photoToUpload);
                
               
                                
            }
            await _context.Photos.AddRangeAsync(productPhotos);
            await _context.SaveChangesAsync();

            foreach (var categoryId in productCreateViewModel.SelectedCategoryIds)
            {
               var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
               product.ProductCategories.Add(category!);
                
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
           

            var photoId = product!.FrontPhotoId;
            await _photoService.DeletePhotoAsync(photoId);
            product!.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(IEnumerable<IFormFile> files, IFormFile file,Guid id,ProductEditViewModel editProductModel)
        {
            var product = await _context.Products.FirstOrDefaultAsync(c => c.Id == id);
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == product!.FrontPhotoId);
            var categories = new List<Category>();
            foreach (var categoryId in editProductModel.SelectedCategoryIds)
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
                categories.Add(category!);
            }
            product!.ProductCategories = categories;
            if (product != null) 
            {
                if (file != null)
                {
                    if (file.FileName != photo!.Name)
                    {
                        var newPhoto = _photoService.CreateImage(file, file.FileName);
                        photo = new Photo
                        {
                            Name = newPhoto.Name,
                            Picture = newPhoto.Picture

                        };
                        await _context.Photos.AddAsync(photo);
                        await _context.SaveChangesAsync();
                        product.FrontPhoto = photo;
                    }
                }
            }
            
            
            
            
            product!.Name = editProductModel.Name;
            product.Description = editProductModel.Description;
            product.Price = editProductModel.Price;
            product.FrontPhotoId = photo!.Id;
            product.Quantity = editProductModel.StockQuantity;
            product.MainCategoryId = editProductModel.CategoryId;
            product.BrandId = editProductModel.BrandId;
            product.ModifiedOn = DateTime.UtcNow;
            product.LongDescription = editProductModel.LongDescription;
            var productPhotos = new List<Photo>();
            foreach (var formFile in files)
            {
                var imageGallery = _photoService.CreateImage(formFile, formFile.FileName);
                var photoToUpload = new Photo
                {
                    Name = imageGallery.Name,
                    Picture = imageGallery.Picture,
                    ProductId = product.Id

                };
                productPhotos.Add(photoToUpload);



            }
            await _context.Photos.AddRangeAsync(productPhotos);
            

            await _context.SaveChangesAsync();
        }

        public async Task<AllProductsFilteredAndPagedServiceModel> GetAllFilteredAndPagedAsync(AllProductsQueryModel productsQueryModel)
        {
            IQueryable<Product> productsQuery = _context
                .Products
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(productsQueryModel.Category))
            {
                productsQuery = productsQuery.Where(p => p.MainCategory.Name == productsQueryModel.Category);
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
                EF.Functions.Like(p.MainCategory.Name.ToString(), searchTermWildCard) ||
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
                    Image = p.FrontPhoto.Picture,
                    StockQuantity = p.Quantity,
                    
                   
                    Category = new CategoryViewModel()
                    {
                        Id = p.MainCategory.Id.ToString(),
                        Name = p.MainCategory.Name
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

        public async Task<ICollection<ProductAllViewModel>> GetLastThreeAddedAsync()
        {
            var products =  await _context.Products.Include(c=>c.MainCategory).Where(p=>p.IsDeleted == false).OrderByDescending(p=>p.CreatedOn).Take(3).Select(p=> new ProductAllViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.Quantity,
                Description = p.Description,
                Image = p.FrontPhoto.Picture,
              
                Category = new CategoryViewModel()
                {
                    Id = p.MainCategory.Id.ToString(),
                    Name = p.MainCategory.Name
                }
            }).ToListAsync();

            return products;
        }

        public async Task<ICollection<ProductAllViewModel>> GetRelatedProductsAsync(Guid categoryId)
        {
            var relatedProducts = await _context.Products.Include(c => c.MainCategory).Where(p => p.MainCategoryId == categoryId && p.IsDeleted == false).Select(p => new ProductAllViewModel
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price,
                StockQuantity = p.Quantity,
                Description = p.Description,
                Image = p.FrontPhoto.Picture,
               
                Category = new CategoryViewModel()
                {
                    Id = p.MainCategory.Id.ToString(),
                    Name = p.MainCategory.Name
                }
            }).Take(5).ToListAsync();

            return relatedProducts;
        }
    }
}
