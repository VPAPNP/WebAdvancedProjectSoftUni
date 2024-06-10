using EShopWebApp.Core.Contracts;
using EShopWebApp.Core.ViewModels.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopWebApp.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : BaseAdminController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IPhotoService _imageService;
        private readonly IPackageService _packageService;

        public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService, IPhotoService imageService, IPackageService packageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _imageService = imageService;
            _packageService = packageService;
        }
        public async Task<IActionResult> Create(ProductCreateFormViewModel productCreateForm)
        {
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var packages = await _packageService.GetAllAsync();
            
            productCreateForm.Categories = categories;
            productCreateForm.Brands = brands;
            productCreateForm.Packages = packages;
            
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(productCreateForm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IEnumerable<IFormFile> files, IFormFile file, ProductCreateViewModel productView)
        {
            

            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
           ModelState.Remove("Package.Name");
            ModelState.Remove("Package.Description");

            
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                var brands = await _brandService.GetAllAsync();
                var packages = await _packageService.GetAllAsync();
                var productCreateForm = new ProductCreateFormViewModel
                {
                    Name = productView.Name,
                    Description = productView.Description,
                    Price = productView.Price,
                    StockQuantity = productView.StockQuantity,
                    BrandId = productView.BrandId,
                    CreatedOn = productView.CreatedOn,
                    CategoryId = productView.CategoryId,
                    Categories = categories,
                    Brands = brands,
                    LongDescription = productView.LongDescription,
                    Packages = packages

                };
                return View(productCreateForm);
            }


            try
            {
                
                await _productService.CreateAsync(files,file, productView);


                return RedirectToAction("All", "Product"); // Redirect to appropriate action
            }
            catch (Exception)
            {
                
                ModelState.AddModelError("Error Create Product", "An error occurred while processing your request.");
                var categories = await _categoryService.GetAllAsync();
                var brands = await _brandService.GetAllAsync();
                var packages = await _packageService.GetAllAsync();
                var productCreateForm = new ProductCreateFormViewModel
                {
                    Name = productView.Name,
                    Description = productView.Description,
                    Price = productView.Price,
                    StockQuantity = productView.StockQuantity,
                    BrandId = productView.BrandId,
                    CreatedOn = productView.CreatedOn,
                    CategoryId = productView.CategoryId,
                    Categories = categories,
                    Brands = brands,
                    LongDescription = productView.LongDescription,
                    Packages = packages

                };
                return View(productCreateForm); 
            }
        }

        public async Task<IActionResult> All()
        {
            var products = await _productService.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction("All", "Product");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var product = await _productService.GetByIdAsync(Guid.Parse(id));
            var categories = await _categoryService.GetAllAsync();
            var packages = await _packageService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var selCategories = await _categoryService.GetAllByProductId(Guid.Parse(id));




            var photos = await _imageService.GetPhotoByProductId(Guid.Parse(id));
           
            var productEditForm = new ProductEditFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                BrandId = Guid.Parse(product.BrandId),
                CreatedOn = product.CreatedOn,
                CategoryId = Guid.Parse(product.CategoryId),
                Categories = categories,
                MainPhotoId =Guid.Parse(product.PhotoId),
                MainPhoto = product.Image,
                Brands = brands,
                Images = photos,
                LongDescription = product.LongDescription,
                SelectedCategoryIds = selCategories.Select(c => Guid.Parse(c.Id)).ToList(),
                Packages = packages
               

            };
            
            return View(productEditForm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(IEnumerable<IFormFile> files,IFormFile file,
            string id,ProductEditViewModel editProductModel)
        {
            
            ModelState.Remove("file");
           


            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();
                var brands = await _brandService.GetAllAsync();
                var packages = await _packageService.GetAllAsync();
                var images = await _imageService.GetPhotoByProductId(Guid.Parse(id));
                var selCategories = await _categoryService.GetAllByProductId(Guid.Parse(id));
                var productEditForm = new ProductEditFormViewModel
                {
                    Name = editProductModel.Name,
                    Description = editProductModel.Description,
                    Price = editProductModel.Price,
                    MainPhotoId = editProductModel.MainPhotoId,
                    StockQuantity = editProductModel.StockQuantity,
                    BrandId = editProductModel.BrandId,
                    CategoryId = editProductModel.CategoryId,
                    Categories = categories,
                    Brands = brands,
                    LongDescription = editProductModel.LongDescription,
                    MainPhoto = editProductModel.MainPhoto,
                    Images = images,
                    SelectedCategoryIds = selCategories.Select(c => Guid.Parse(c.Id)).ToList()
                };
                await Console.Out.WriteLineAsync();
                return View(productEditForm);
            }
            await _productService.EditAsync(files,file,Guid.Parse(id), editProductModel);
            return RedirectToAction("All", "Product");
        }
    }
}
